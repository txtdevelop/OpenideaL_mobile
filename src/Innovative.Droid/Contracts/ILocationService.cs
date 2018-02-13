using System.Threading;
using System.Threading.Tasks;
using Android.Locations;

namespace PSY.Innovative.Droid.Contracts
{
    public interface ILocationService
    {
        /// <summary>
        /// Gets the location async.
        /// </summary>
        /// <returns>The location async.</returns>
        Task<Location> GetLocationAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:PSY.Innovative.Droid.Contracts.ILocationService"/> is location available.
        /// </summary>
        /// <value><c>true</c> if is location available; otherwise, <c>false</c>.</value>
        bool IsLocationAvailable { get; }

        /// <summary>
        /// Updates the street address async.
        /// </summary>
        /// <returns>True if successfull.</returns>
        /// <param name="location">Location.</param>
        Task<bool> UpdateStreetAddressAsync(Location location);
    }

   

}

