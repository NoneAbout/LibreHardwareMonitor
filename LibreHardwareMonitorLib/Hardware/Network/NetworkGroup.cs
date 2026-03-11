using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using LibreHardwareMonitor.Hardware;

namespace LibreHardwareMonitorLib.Hardware.Network
{
    internal class Network : Hardware
    {
        private readonly Sensor _load;
        private readonly Sensor _dataDownload;
        private readonly Sensor _dataUpload;
        private readonly Sensor _bandwidth; // Yeni eklediğimiz Link Speed sensörü
        private readonly NetworkInterface _networkInterface;

        public Network(NetworkInterface networkInterface, Identifier identifier, ISettings settings)
            : base(networkInterface.Name, identifier, settings)
        {
            _networkInterface = networkInterface;

            // Sensörleri tanımlıyoruz
            _load = new Sensor("Network Load", 0, SensorType.Load, this, settings);
            _dataDownload = new Sensor("Download Speed", 1, SensorType.Data, this, settings);
            _dataUpload = new Sensor("Upload Speed", 2, SensorType.Data, this, settings);
            
            // Link Speed (Bağlantı Hızı) Sensörü - Mbps cinsinden
            _bandwidth = new Sensor("Link Speed", 3, SensorType.Factor, this, settings);

            ActivateSensor(_load);
            ActivateSensor(_dataDownload);
            ActivateSensor(_dataUpload);
            ActivateSensor(_bandwidth);
        }

        public override HardwareType HardwareType => HardwareType.Network;

        public override void Update()
        {
            try 
            {
                // Bağlantı hızını çekiyoruz (bps -> Mbps dönüşümü)
                // 1000000'a bölerek 10, 100 veya 1000 değerini elde ederiz.
                _bandwidth.Value = (float)(_networkInterface.Speed / 1,000,000.0);

                // Diğer trafik verilerini güncelle (mevcut LHM kodları buraya gelecek)
                // ... (mevcut Update kodların)
            }
            catch (Exception) 
            {
                // Hata durumunda boş geç
            }
        }
    }
}
