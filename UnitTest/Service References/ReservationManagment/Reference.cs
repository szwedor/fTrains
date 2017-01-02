﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UnitTest.ReservationManagment {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReservationManagment.IReservationManagment")]
    public interface IReservationManagment {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagmentUnsecure/AllStations", ReplyAction="http://tempuri.org/IReservationManagmentUnsecure/AllStationsResponse")]
        DomainModel.Models.Station[] AllStations();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagmentUnsecure/AllStations", ReplyAction="http://tempuri.org/IReservationManagmentUnsecure/AllStationsResponse")]
        System.Threading.Tasks.Task<DomainModel.Models.Station[]> AllStationsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagmentUnsecure/FindConnection", ReplyAction="http://tempuri.org/IReservationManagmentUnsecure/FindConnectionResponse")]
        DomainModel.Models.Connection[][] FindConnection(DomainModel.Models.Station departure, DomainModel.Models.Station arrival, System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagmentUnsecure/FindConnection", ReplyAction="http://tempuri.org/IReservationManagmentUnsecure/FindConnectionResponse")]
        System.Threading.Tasks.Task<DomainModel.Models.Connection[][]> FindConnectionAsync(DomainModel.Models.Station departure, DomainModel.Models.Station arrival, System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagment/MakeReservation", ReplyAction="http://tempuri.org/IReservationManagment/MakeReservationResponse")]
        int MakeReservation(DomainModel.Models.Connection con, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagment/MakeReservation", ReplyAction="http://tempuri.org/IReservationManagment/MakeReservationResponse")]
        System.Threading.Tasks.Task<int> MakeReservationAsync(DomainModel.Models.Connection con, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagment/DeleteReservation", ReplyAction="http://tempuri.org/IReservationManagment/DeleteReservationResponse")]
        void DeleteReservation(string userName, DomainModel.Models.Ticket t);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagment/DeleteReservation", ReplyAction="http://tempuri.org/IReservationManagment/DeleteReservationResponse")]
        System.Threading.Tasks.Task DeleteReservationAsync(string userName, DomainModel.Models.Ticket t);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagment/AllUserReservations", ReplyAction="http://tempuri.org/IReservationManagment/AllUserReservationsResponse")]
        DomainModel.Models.Ticket[] AllUserReservations(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagment/AllUserReservations", ReplyAction="http://tempuri.org/IReservationManagment/AllUserReservationsResponse")]
        System.Threading.Tasks.Task<DomainModel.Models.Ticket[]> AllUserReservationsAsync(string userName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IReservationManagmentChannel : UnitTest.ReservationManagment.IReservationManagment, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ReservationManagmentClient : System.ServiceModel.ClientBase<UnitTest.ReservationManagment.IReservationManagment>, UnitTest.ReservationManagment.IReservationManagment {
        
        public ReservationManagmentClient() {
        }
        
        public ReservationManagmentClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ReservationManagmentClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReservationManagmentClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReservationManagmentClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public DomainModel.Models.Station[] AllStations() {
            return base.Channel.AllStations();
        }
        
        public System.Threading.Tasks.Task<DomainModel.Models.Station[]> AllStationsAsync() {
            return base.Channel.AllStationsAsync();
        }
        
        public DomainModel.Models.Connection[][] FindConnection(DomainModel.Models.Station departure, DomainModel.Models.Station arrival, System.DateTime date) {
            return base.Channel.FindConnection(departure, arrival, date);
        }
        
        public System.Threading.Tasks.Task<DomainModel.Models.Connection[][]> FindConnectionAsync(DomainModel.Models.Station departure, DomainModel.Models.Station arrival, System.DateTime date) {
            return base.Channel.FindConnectionAsync(departure, arrival, date);
        }
        
        public int MakeReservation(DomainModel.Models.Connection con, string userName) {
            return base.Channel.MakeReservation(con, userName);
        }
        
        public System.Threading.Tasks.Task<int> MakeReservationAsync(DomainModel.Models.Connection con, string userName) {
            return base.Channel.MakeReservationAsync(con, userName);
        }
        
        public void DeleteReservation(string userName, DomainModel.Models.Ticket t) {
            base.Channel.DeleteReservation(userName, t);
        }
        
        public System.Threading.Tasks.Task DeleteReservationAsync(string userName, DomainModel.Models.Ticket t) {
            return base.Channel.DeleteReservationAsync(userName, t);
        }
        
        public DomainModel.Models.Ticket[] AllUserReservations(string userName) {
            return base.Channel.AllUserReservations(userName);
        }
        
        public System.Threading.Tasks.Task<DomainModel.Models.Ticket[]> AllUserReservationsAsync(string userName) {
            return base.Channel.AllUserReservationsAsync(userName);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ReservationManagment.IReservationManagmentUnsecure")]
    public interface IReservationManagmentUnsecure {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagmentUnsecure/AllStations", ReplyAction="http://tempuri.org/IReservationManagmentUnsecure/AllStationsResponse")]
        DomainModel.Models.Station[] AllStations();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagmentUnsecure/AllStations", ReplyAction="http://tempuri.org/IReservationManagmentUnsecure/AllStationsResponse")]
        System.Threading.Tasks.Task<DomainModel.Models.Station[]> AllStationsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagmentUnsecure/FindConnection", ReplyAction="http://tempuri.org/IReservationManagmentUnsecure/FindConnectionResponse")]
        DomainModel.Models.Connection[][] FindConnection(DomainModel.Models.Station departure, DomainModel.Models.Station arrival, System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReservationManagmentUnsecure/FindConnection", ReplyAction="http://tempuri.org/IReservationManagmentUnsecure/FindConnectionResponse")]
        System.Threading.Tasks.Task<DomainModel.Models.Connection[][]> FindConnectionAsync(DomainModel.Models.Station departure, DomainModel.Models.Station arrival, System.DateTime date);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IReservationManagmentUnsecureChannel : UnitTest.ReservationManagment.IReservationManagmentUnsecure, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ReservationManagmentUnsecureClient : System.ServiceModel.ClientBase<UnitTest.ReservationManagment.IReservationManagmentUnsecure>, UnitTest.ReservationManagment.IReservationManagmentUnsecure {
        
        public ReservationManagmentUnsecureClient() {
        }
        
        public ReservationManagmentUnsecureClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ReservationManagmentUnsecureClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReservationManagmentUnsecureClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReservationManagmentUnsecureClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public DomainModel.Models.Station[] AllStations() {
            return base.Channel.AllStations();
        }
        
        public System.Threading.Tasks.Task<DomainModel.Models.Station[]> AllStationsAsync() {
            return base.Channel.AllStationsAsync();
        }
        
        public DomainModel.Models.Connection[][] FindConnection(DomainModel.Models.Station departure, DomainModel.Models.Station arrival, System.DateTime date) {
            return base.Channel.FindConnection(departure, arrival, date);
        }
        
        public System.Threading.Tasks.Task<DomainModel.Models.Connection[][]> FindConnectionAsync(DomainModel.Models.Station departure, DomainModel.Models.Station arrival, System.DateTime date) {
            return base.Channel.FindConnectionAsync(departure, arrival, date);
        }
    }
}