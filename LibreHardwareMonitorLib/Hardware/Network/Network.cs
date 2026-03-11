using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using LibreHardwareMonitor.Hardware;

namespace LibreHardwareMonitorLib.Hardware.Network
{
    // Hata almamak için tam yoluyla (Full Path) belirttik
    internal class Network : LibreHardwareMonitor.Hardware.Hardware
    {
        private readonly Sensor _load;
        private readonly Sensor _dataDownload;
        private readonly Sensor _dataUpload;
        private readonly Sensor _linkSpeed; // Yeni sensörümüz
        private readonly NetworkInterface _networkInterface;

        public Network(NetworkInterface networkInterface, Identifier identifier, ISettings settings)
            : base(networkInterface.Name, identifier, settings)
        {
            _networkInterface = networkInterface;

            _load = new Sensor("Network Load", 0, SensorType.Load, this, settings);
            _dataDownload = new Sensor("Download Speed", 1, SensorType.Data, this, settings);
            _dataUpload = new Sensor("Upload Speed", 2, SensorType.Data, this, settings);
            
            // Link Speed (Mbps) - 3 numaralı index ile ekliyoruz
            _linkSpeed = new Sensor("Link Speed", 3, SensorType.Factor, this, settings);

            ActivateSensor(_load);
            ActivateSensor(_dataDownload);
            ActivateSensor(_dataUpload);
            ActivateSensor(_linkSpeed);
        }

        public override HardwareType HardwareType => HardwareType.Network;

        public override void Update()
        {
            try 
            {
                // nic.Speed bps (bit per second) döner. 
                // 1,000,000'a bölerek Mbps elde ediyoruz.
                if (_networkInterface != null)
                {
                    _linkSpeed.Value = (float)(_networkInterface.Speed / 1000000.0f);
                }
            }
            catch (Exception) 
            {
                // Hata olursa değeri sıfırla veya eski değerde bırak
            }
        }
    }
}
