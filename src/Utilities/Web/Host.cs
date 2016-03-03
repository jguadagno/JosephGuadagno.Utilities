using System;
using System.Diagnostics;

namespace JosephGuadagno.Utilities.Web
{
    /// <summary>
    ///     Maps out host information
    /// </summary>
    [DebuggerDisplay("SubDomain={SubDomain} Domain={Domain} Port={Port}")]
    public class Host
    {
        public Host()
        {
            Domain = string.Empty;
            SubDomain = string.Empty;
            Port = string.Empty;
        }

        /// <summary>
        ///     Gets or sets the domain name.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain { get; set; }

        /// <summary>
        ///     Gets or sets the sub domain.
        /// </summary>
        /// <value>The sub domain.</value>
        public string SubDomain { get; set; }

        /// <summary>
        ///     Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public string Port { get; set; }


        /// <summary>
        ///     Parses a string into a Host object
        /// </summary>
        /// <param name="serverName">The Request.Params["SERVER_NAME"]</param>
        /// <returns>A Host object</returns>
        public static Host ParseDomain(string serverName)
        {
            if (string.IsNullOrEmpty(serverName))
                return new Host {SubDomain = "", Domain = "", Port = ""};

            // Determine the port
            var port = getPort(serverName);
            var subDomain = string.Empty;
            var serverNameLessPort = (string.IsNullOrEmpty(port))
                ? serverName
                : serverName.Replace(":", "").Replace(port, "");

            // get the sub domain and parts
            var parts = serverNameLessPort.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                if (parts[1] == "localhost")
                    return new Host {Port = port, SubDomain = parts[0], Domain = parts[1]};
                return new Host {SubDomain = subDomain, Port = port, Domain = serverNameLessPort};
            }
            if (parts.Length < 2)
            {
                return new Host {SubDomain = subDomain, Port = port, Domain = serverNameLessPort};
            }
            // subdomain.domain.com
            string domain = parts[parts.Length - 2] + "." + parts[parts.Length - 1];
            for (int i = 0; i < parts.Length - 2; i++)
            {
                subDomain = subDomain + parts[i] + ".";
            }
            subDomain = subDomain.Substring(0, subDomain.Length - 1);

            return new Host {SubDomain = subDomain, Domain = domain, Port = port};
        }

        private static string getPort(string serverName)
        {
            if (string.IsNullOrEmpty(serverName)) return string.Empty;
            var portIndex = serverName.IndexOf(":", StringComparison.Ordinal);
            return portIndex == -1 ? string.Empty : serverName.Substring(portIndex + 1);
        }
    }
}