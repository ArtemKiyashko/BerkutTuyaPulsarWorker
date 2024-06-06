using BerkutTuyaPulsarWorker.Models;

namespace BerkutTuyaPulsarWorker.Interfaces;

public interface IBroadcasterRepository
{
    public Task PublishDeviceDataAsync(DeviceData deviceData);
}
