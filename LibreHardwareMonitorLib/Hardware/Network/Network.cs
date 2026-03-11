using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using LibreHardwareMonitor.Hardware;

namespace LibreHardwareMonitorLib.Hardware.Network
{
    internal class Network : LibreHardwareMonitor.Hardware.Hardware
    {
        private readonly Sensor _load;
        private readonly Sensor _dataDownload;
        private readonly Sensor _dataUpload;
        private readonly Sensor _linkSpeed; 
        private readonly NetworkInterface _networkInterface;

        public Network(NetworkInterface networkInterface, Identifier identifier, ISettings settings)
            : base(networkInterface.Name, identifier, settings)
        {
            _networkInterface = networkInterface;

            _load = new Sensor("Network Load", 0, SensorType.Load, this, settings);
            _dataDownload = new Sensor("Download Speed", 1, SensorType.Data, this, settings);
            _dataUpload = new Sensor("Upload Speed", 2, SensorType.Data, this, settings);
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
                if (_networkInterface != null)
                {
                    // bps -> Mbps (1,000,000'a bölüyoruz)
                    _linkSpeed.Value = (float)(_networkInterface.Speed / 1000000.0f);
                }
            }
            catch (Exception) { }
        }
    }
}
