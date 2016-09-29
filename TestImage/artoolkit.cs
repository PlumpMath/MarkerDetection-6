namespace TestImage
{
    class artoolkit
    {
        private int UNKNOWN_MARKER = -1;
        private int PATTERN_MARKER = 0;
        private int BARCODE_MARKER = 1;

        private string[] FUNCTIONS;


        public artoolkit()
        {
            FUNCTIONS = new string[37]{
            "setup", "teardown", "setLogLevel", "getLogLevel", "setDebugMode", "getDebugMode", "getProcessingImage",
            "setMarkerInfoDir", "setMarkerInfoVertex", "getTransMatSquare", "getTransMatSquareCont",
            "getTransMatMultiSquare", "getTransMatMultiSquareRobust", "getMultiMarkerNum", "getMultiMarkerCount",
            "detectMarker", "getMarkerNum", "getMarker", "getMultiEachMarker", "setProjectionNearPlane",
            "getProjectionNearPlane", "setProjectionFarPlane", "getProjectionFarPlane", "setThresholdMode",
            "getThresholdMode", "setThreshold", "getThreshold", "setPatternDetectionMode", "getPatternDetectionMode",
            "setMatrixCodeType", "getMatrixCodeType", "setLabelingMode", "getLabelingMode", "setPattRatio",
            "getPattRatio", "setImageProcMode", "getImageProcMode"
        };


        }

       
    }
}