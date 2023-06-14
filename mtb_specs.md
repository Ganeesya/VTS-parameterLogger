# Structure

|Item|Data Size|Format|Content|
|-|-|-|-|
|Header Reserved Word|3byte|Character|The reserved word 'MTB'<br>Indicates that this is an MTB file.|
|File Version|1byte|char|Indicates the file version of MTB.<br>Currently only 0.|
|◆Video Information|12byte|Structure|FPS and total frame count|
|◯Motion Composition Information|Variable|Structure|Number and ID list of parameters, parts|
|■Frame Information|Variable|Array of Structures|Parameter values, part values for each frame|

## ◆Video Information

|Item|Data Size|Format|Content|
|-|-|-|-|
|FPS (frames per second)|4byte|Float|Number of frames per second|
|Saved Frame Count|8byte|Long|Number of frames saved.|

## ◯Motion Composition Information

|Item|Data Size|Format|Content|
|-|-|-|-|
|Number of Parameters|4byte|int|Number of parameters|
|Number of Parts|4byte|int|Number of parts<br>Note that this is not currently output in VTubeStudioAccess|
|Parameter ID Group|△ID x Number of Parameters|Array of IDs|List of parameter IDs|
|Parts ID Group|△ID x Number of Parts|Array of IDs|List of parts IDs<br>Note that this is not currently output in VTubeStudioAccess|

### △ID

|Item|Data Size|Format|Content|
|-|-|-|-|
|Number of Characters|1byte|char|Length of the following character information<br>Since Live2D's ID is 64 characters, it is within 64.|
|Character|Varies depending on the number of characters|String|ID content|

## ■Frame Information

|Item|Data Size|Format|Content|
|-|-|-|-|
|Parameter Value List|4byte x Number of Parameters|Array of Floats|List of parameter values for that frame|
|Parts Value List|4byte x Number of Parts|Array of Floats|List of parts values for that frame|