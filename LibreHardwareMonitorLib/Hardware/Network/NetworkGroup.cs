using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace LibreHardwareMonitorLib.Hardware.Network
{
    internal class NetworkGroup : IGroup
    {
        private readonly List<Network> _hardware = new List<Network>();

        public NetworkGroup(ISettings settings)
        {
            if (!NetworkInterface.GetIsNetworkInterfaceAvailable())
                return;

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in interfaces)
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;

                _hardware.Add(new Network(networkInterface, new Identifier("network", networkInterface.Id), settings));
            }
        }

        public IEnumerable<IHardware> Hardware => _hardware;

        public void Close()
        {
            foreach (Network network in _hardware)
                network.Close();
        }
    }
}
