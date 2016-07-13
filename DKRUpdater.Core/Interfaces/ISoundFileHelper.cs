namespace DKRUpdater.Core.Interfaces
{
    public interface ISoundFileHelper
    {
        /// <summary>
        /// Supports .m4a, .wav and .mp3
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        string ToMp3(string pathToFile); 
    }
}
