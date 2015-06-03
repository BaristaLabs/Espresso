namespace BaristaLabs.Espresso.Common
{
    /// <summary>
    /// Represents an undefined value.
    /// </summary>
    public class Undefined
    {
        public static readonly Undefined Value = new Undefined();

        private Undefined()
        {
        }

        #region Object overrides

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        /// <remarks>
        /// The <see cref="Undefined"/> version of this method returns "[undefined]".
        /// </remarks>
        public override string ToString()
        {
            return "[undefined]";
        }

        #endregion
    }
}
