namespace ContactGloveSDK
{
    internal interface IGloveDataController
    {
        /// <summary>
        /// Set fingerRotationAmplitude[hand, index] to amplitude
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="index"></param>
        /// <param name="amplitude"></param>
        void SetFingerRotationAmplitude(HandSides hand, FingerRotationAmplitude_e index, float amplitude);
    }
}
