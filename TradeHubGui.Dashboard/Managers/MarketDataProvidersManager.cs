﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TradeHubGui.Common.Models;

namespace TradeHubGui.Dashboard.Managers
{
    /// <summary>
    /// Handles Market Data Provider's related Admin functionality
    /// </summary>
    public class MarketDataProvidersManager
    {
        private Type _type = typeof(MarketDataProvidersManager);

        /// <summary>
        /// Directory path at which Market Data Provider's files are located
        /// </summary>
        private readonly string _marketDataProvidersFolderPath;

        /// <summary>
        /// File name which holds the name of all available market data providers
        /// </summary>
        private readonly string _marketDataProvidersFileName;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MarketDataProvidersManager()
        {
            _marketDataProvidersFolderPath =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                "\\TradeHub\\MarketDataProviders\\";

            _marketDataProvidersFileName = "AvailableProviders.xml";
        }

        /// <summary>
        /// Returns a list of available market data providers
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, List<ProviderCredential>> GetAvailableProviders()
        {
            // File Saftey Check
            if (!File.Exists(_marketDataProvidersFolderPath + _marketDataProvidersFileName)) 
                return null;

            // Will hold credential information against each availale provider
            IDictionary<string, List<ProviderCredential>> availableProviders = new Dictionary<string, List<ProviderCredential>>();

            // XML Document to read file
            var availableProvidersDocument = new XmlDocument();

            // Read file to get available provider's names.
            availableProvidersDocument.Load(_marketDataProvidersFolderPath + _marketDataProvidersFileName);

            // Read the all Node value
            XmlNodeList providersInfo = availableProvidersDocument.SelectNodes(xpath: "Providers/*");

            if (providersInfo != null)
            {
                // Extract individual attribute value
                foreach (XmlNode node in providersInfo)
                {
                    // Create file name from which to read Provider Credentials
                    string credentialsFileName = node.Name + @"Params.xml";

                    // XML Document to read provider specific xml file
                    var availableCredentialsDoc = new XmlDocument();

                    // Holds extracted credentials from the xml file
                    var providerCredentialList = new List<ProviderCredential>();

                    if (File.Exists(_marketDataProvidersFolderPath + credentialsFileName))
                    {
                        // Read configuration file
                        availableCredentialsDoc.Load(_marketDataProvidersFolderPath + credentialsFileName);

                        // Read all the parametes defined in the configuration file
                        XmlNodeList configNodes = availableCredentialsDoc.SelectNodes(xpath: node.Name + "/*");
                        if (configNodes != null)
                        {
                            // Extract individual attribute value
                            foreach (XmlNode innerNode in configNodes)
                            {
                                ProviderCredential providerCredential = new ProviderCredential();

                                providerCredential.CredentialName = innerNode.Name;
                                providerCredential.CredentialValue = innerNode.InnerText;

                                // Add to Credentials list
                                providerCredentialList.Add(providerCredential);
                            }
                        }
                    }
                    // Add all details to providers info map
                    availableProviders.Add(node.Name, providerCredentialList);
                }
            }

            return availableProviders;
        }
    }
}
