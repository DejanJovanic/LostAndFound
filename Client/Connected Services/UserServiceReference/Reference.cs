﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.UserServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Person", Namespace="http://schemas.datacontract.org/2004/07/Server.Model")]
    [System.SerializableAttribute()]
    public partial class Person : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsAdminField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsernameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsAdmin {
            get {
                return this.IsAdminField;
            }
            set {
                if ((this.IsAdminField.Equals(value) != true)) {
                    this.IsAdminField = value;
                    this.RaisePropertyChanged("IsAdmin");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName {
            get {
                return this.LastNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LastNameField, value) != true)) {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Username {
            get {
                return this.UsernameField;
            }
            set {
                if ((object.ReferenceEquals(this.UsernameField, value) != true)) {
                    this.UsernameField = value;
                    this.RaisePropertyChanged("Username");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResponseCode", Namespace="http://schemas.datacontract.org/2004/07/Server.Interfaces")]
    public enum ResponseCode : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OK = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        USERNAMETAKEN = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        USERNOTFOUND = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        INVALIDRIGHT = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        BADUSERSUPPLIED = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NOTLOGGEDIN = 5,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UserServiceReference.IUserService")]
    public interface IUserService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/AddPerson", ReplyAction="http://tempuri.org/IUserService/AddPersonResponse")]
        Client.UserServiceReference.ResponseCode AddPerson(Client.UserServiceReference.Person person, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/AddPerson", ReplyAction="http://tempuri.org/IUserService/AddPersonResponse")]
        System.Threading.Tasks.Task<Client.UserServiceReference.ResponseCode> AddPersonAsync(Client.UserServiceReference.Person person, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/UpdatePerson", ReplyAction="http://tempuri.org/IUserService/UpdatePersonResponse")]
        Client.UserServiceReference.ResponseCode UpdatePerson(string name, string lastName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/UpdatePerson", ReplyAction="http://tempuri.org/IUserService/UpdatePersonResponse")]
        System.Threading.Tasks.Task<Client.UserServiceReference.ResponseCode> UpdatePersonAsync(string name, string lastName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/RemovePerson", ReplyAction="http://tempuri.org/IUserService/RemovePersonResponse")]
        Client.UserServiceReference.ResponseCode RemovePerson(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/RemovePerson", ReplyAction="http://tempuri.org/IUserService/RemovePersonResponse")]
        System.Threading.Tasks.Task<Client.UserServiceReference.ResponseCode> RemovePersonAsync(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/GetAllPersons", ReplyAction="http://tempuri.org/IUserService/GetAllPersonsResponse")]
        Client.UserServiceReference.Person[] GetAllPersons();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/GetAllPersons", ReplyAction="http://tempuri.org/IUserService/GetAllPersonsResponse")]
        System.Threading.Tasks.Task<Client.UserServiceReference.Person[]> GetAllPersonsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUserServiceChannel : Client.UserServiceReference.IUserService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserServiceClient : System.ServiceModel.ClientBase<Client.UserServiceReference.IUserService>, Client.UserServiceReference.IUserService {
        
        public UserServiceClient() {
        }
        
        public UserServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UserServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Client.UserServiceReference.ResponseCode AddPerson(Client.UserServiceReference.Person person, string password) {
            return base.Channel.AddPerson(person, password);
        }
        
        public System.Threading.Tasks.Task<Client.UserServiceReference.ResponseCode> AddPersonAsync(Client.UserServiceReference.Person person, string password) {
            return base.Channel.AddPersonAsync(person, password);
        }
        
        public Client.UserServiceReference.ResponseCode UpdatePerson(string name, string lastName) {
            return base.Channel.UpdatePerson(name, lastName);
        }
        
        public System.Threading.Tasks.Task<Client.UserServiceReference.ResponseCode> UpdatePersonAsync(string name, string lastName) {
            return base.Channel.UpdatePersonAsync(name, lastName);
        }
        
        public Client.UserServiceReference.ResponseCode RemovePerson(string username) {
            return base.Channel.RemovePerson(username);
        }
        
        public System.Threading.Tasks.Task<Client.UserServiceReference.ResponseCode> RemovePersonAsync(string username) {
            return base.Channel.RemovePersonAsync(username);
        }
        
        public Client.UserServiceReference.Person[] GetAllPersons() {
            return base.Channel.GetAllPersons();
        }
        
        public System.Threading.Tasks.Task<Client.UserServiceReference.Person[]> GetAllPersonsAsync() {
            return base.Channel.GetAllPersonsAsync();
        }
    }
}
