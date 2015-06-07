namespace BaristaLabs.Espresso.Common
{
    using System.IO;

    public interface ISerializer
    {
        /// <summary>
        /// Serialize the given object.
        /// </summary>
        /// <param name="outputStream">Output stream to serialize to</param>
        /// <param name="obj">Model to serialize</param>
        /// <returns>Serialized object</returns>
        void Serialize(Stream outputStream, object obj);
    }
}
