namespace BaristaLabs.Espresso.Core
{
    using Nancy;

    /// <summary>
    /// Represents a Barista -- an object that is responsible for taking a request, brewing up some JavaScript and returning a result.
    /// </summary>
    /// <remarks>
    /// Various Barista impementations may be used for different purposes, specialized brews, for instance.
    /// </remarks>
    public interface IBarista
    {

        Response Brew(NancyContext ctx);
    }
}