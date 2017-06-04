# MarkerDetection
This repository is documented at Food4Rhino called **Falcon**. Please refer to [Food4Rhino](http://www.food4rhino.com/app/falcon) for further info.

This is a grasshopper plug-in that deals with **Conversion among Matrix4x4, Quaternion and Euler** in grasshopper, as well as offers **Marker Detection** to get position of the markers. <br>

It has 4 parts. First part is the calculation among 4x4 Matrix, convert from other rotation definition to Matrix. Second part is the calculation among Quaternion, convert from other rotation definition to Quaternion. Third part is Marker Detection, Load image as System Bitmap, Show image, Webcam, and conversion between firefly image in case you need to use some functions from firefly. Fourth part is a path finding component that seeks shortest path between two points with given obstacles.

To use marker detection, you need to print out your markers or use it on your pad or phone(not recommended due to strong reflection of screen). Current supported markers are [AprilTags](http://www.dotproduct3d.com/assets/pdf/apriltags.pdf) and [NyID](http://sixwish.jp/AR/Marker/idMarker/). Remember, a white boarder around marker is necessary for detection. (markers should have at least one white spot at each row and column, due to AForge limitation. So not all the markers in above links would work.)

### Acknowledgements:

The matrix and quaternion calculations are realized using ```.NET Framework 4.6```. The marker detection is developed based on ```Aforge.Net``` Framework. Also thanks to Long Nguyen, researcher from ICD, University of Stuttgart, who helped debug my plugin. Thanks to Behrooz Tahanzadeh for me to come up with this idea.

I have tested this project, but it is the first release and it might contain still bugs. Please use it "as is", it does not come with warranties. I spent a lot of time developing the logic and implementing it, please give credit where credit is due.

### To install:

*The run the plugin you need at least net framework 4.6.
*To let conversion between bitmap and firefly image work, you need firefly plugin. [optional]
*In Grasshopper, choose File > Special Folders > Components folder. Save the Falcon.gha and Aforge.dll file there.
*Right-click the file > Properties > make sure there is no "blocked" text
*Restart Rhino and Grasshopper
*In order to use marker detection, you need to download markers from [AprilTags](http://www.dotproduct3d.com/assets/pdf/apriltags.pdf) or [NyID](http://sixwish.jp/AR/Marker/idMarker/). Currently the plugin support Apriltags and NyID, in case you want to use specific marker please contact me to add the database.
