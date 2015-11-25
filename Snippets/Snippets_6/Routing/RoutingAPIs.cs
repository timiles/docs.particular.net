﻿namespace Snippets6.Routing
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Routing;

    public class RoutingAPIs
    {

        public void StaticRoutes()
        {
            var busConfig = new BusConfiguration();
            #region Routing-StaticRoutes
            UnicastRoutingTable routingTable = busConfig.Routing().UnicastRoutingTable;
            routingTable.AddStatic(typeof(OrderAccepted), new EndpointName("Sales"));
            routingTable.AddStatic(typeof(OrderAccepted), new EndpointInstanceName(new EndpointName("Sales"), "1", null));
            routingTable.AddStatic(typeof(OrderAccepted), "Sales-2@MachineA");
            #endregion
        }

        public void DynamicRoutes()
        {
            var busConfig = new BusConfiguration();
            #region Routing-DynamicRoutes
            busConfig.Routing().UnicastRoutingTable.AddDynamic((t, c) => new[]
            {
                //Use endpoint name
                new UnicastRoute(new EndpointName("Sales")), 
                //Use endpoint instance name
                new UnicastRoute(new EndpointInstanceName(new EndpointName("Sales"), "1", null)), 
                //Use transport address (e.g. MSMQ)
                new UnicastRoute("Sales-2@MachineA"),
            });
            #endregion
        }

        public void CustomRoutingStore()
        {
            var busConfig = new BusConfiguration();
            #region Routing-CustomRoutingStore
            busConfig.Routing().UnicastRoutingTable.AddDynamic((t, c) => {
                return LoadFromCache(t) ?? LoadFromDatabaseAndPutToCache(t);
            });
            #endregion
        }

        public void StaticEndpointMapping()
        {
            var busConfig = new BusConfiguration();
            #region Routing-StaticEndpointMapping
            EndpointName sales = new EndpointName("Sales");
            busConfig.Routing().EndpointInstances
                .AddStatic(sales, new EndpointInstanceName(sales, "1", null), new EndpointInstanceName(sales, "2", null));
            #endregion
        }

        public void DynamicEndpointMapping()
        {
            var busConfig = new BusConfiguration();
            #region Routing-DynamicEndpointMapping
            EndpointName sales = new EndpointName("Sales");
            busConfig.Routing().EndpointInstances.AddDynamic(e =>
            {
                if (e.ToString().StartsWith("Sales"))
                {
                    return new[]
                    {
                        new EndpointInstanceName(e, "1", "MachineA"),
                        new EndpointInstanceName(e, "2", "MachineB")
                    };
                }
                return null;
            });
            #endregion
        }

        public void SpecialCaseTransportAddress()
        {
            var busConfig = new BusConfiguration();
            #region Routing-SpecialCaseTransportAddress
            EndpointName sales = new EndpointName("Sales");
            busConfig.Routing().TransportAddresses
                .AddSpecialCase(new EndpointInstanceName(sales, "1", null), "Sales-One@MachineA");
            #endregion
        }

        // ReSharper disable once ConvertClosureToMethodGroup
        public void TransportAddressRules()
        {
            var busConfig = new BusConfiguration();
            #region Routing-TransportAddressRule
            EndpointName sales = new EndpointName("Sales");
            busConfig.Routing().TransportAddresses.AddRule(i => CustomTranslationRule(i));
            #endregion
        }

        string CustomTranslationRule(EndpointInstanceName endpointInstanceName)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IUnicastRoute> LoadFromDatabaseAndPutToCache(Type type)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IUnicastRoute> LoadFromCache(Type type)
        {
            throw new NotImplementedException();
        }

        class OrderAccepted
        {
        }
    }
}