namespace BaristaLabs.Espresso.Common
{
    /// <summary>
    /// Represents a Barista -- an object that is responsible for taking a request, brewing up some JavaScript and returning a result.
    /// </summary>
    /// <remarks>
    /// Various Barista impementations may be used for different purposes, specialized brews, for instance.
    /// </remarks>
    public interface IBarista
    {
        /// <summary>
        /// Returns a response based on the brew request.
        /// </summary>
        /// <remarks>
        /// I can smell the coffee!
        /// </remarks>
        /// <returns></returns>
        IBrewResponse Brew(IBrewRequest request);
    }
}