local ffi = require("ffi")
ffi.cdef([[
    void StockParameterFromVTS(const char * logBaseDirPath, float fps);
    void StopRecord();
]])
local mylib = ffi.load("VTubeStudioAccess.dll")

obs = obslua
local env = {isSaving = false, isSteaming = false, isStartUp = false}

function script_properties()
    local properties = obs.obs_properties_create()
    local file_prop = obs.obs_properties_add_path(properties, "file_path", "ファイルパス", obs.OBS_PATH_DIRECTORY , "", nil)
    local fps_prop = obs.obs_properties_add_int(properties, "fps_int", "FPS", 10, 120, 1)

    return properties
end

function script_save(settings)
    env.path = obs.obs_data_get_string(settings, "file_path")
    env.fps = obs.obs_data_get_int(settings, "fps_int")
end

function script_update(settings)
    env.path = obs.obs_data_get_string(settings, "file_path")
    env.fps = obs.obs_data_get_int(settings, "fps_int")
end


function script_description()
    return "Save VTube Studio Parameters."
end 

function on_event(event)
    if event == obs.OBS_FRONTEND_EVENT_RECORDING_STARTED then 
        env.isSaving = true 
    elseif event == obs.OBS_FRONTEND_EVENT_RECORDING_STOPPED then 
        env.isSaving = false 
    elseif event == obs.OBS_FRONTEND_EVENT_STREAMING_STARTED then 
        env.isSteaming = true 
    elseif event == obs.OBS_FRONTEND_EVENT_STREAMING_STOPPED then 
        env.isSteaming = false 
    end 
end 

function script_tick(seconds)
    -- startUpフラグは起動時のFFI内でのクラッシュを避けるために設けている。
    if not (env.isSaving or env.isSteaming or env.isStartUp) then return end 
    env.isStartUp = true

    if env.isSaving or env.isSteaming then
        if env.path then 
            mylib.StockParameterFromVTS(env.path,env.fps);
        end 
    else
        mylib.StopRecord()
    end
end 

function script_load(settings)
    obs.obs_frontend_add_event_callback(on_event)
    obs.obs_data_set_default_int(settings,"fps_int", 60)
    env.path = obs.obs_data_get_string(settings, "file_path")
    env.fps = obs.obs_data_get_int(settings, "fps_int")
end

function script_unload()
    mylib.StopRecord()
end
