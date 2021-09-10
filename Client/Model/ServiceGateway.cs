using Client.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Model
{
    class ServiceGateway : IServiceGateway
    {
        private DataChange data;
        IUserData userData;
        private DataService dataService;
        private PersonService personService;
        private LoginUpdateService loginUpdate;

        public ServiceGateway(IUserData userData)
        {
            data = new DataChange();
            this.userData = userData;
            loginUpdate = new LoginUpdateService(data, userData);

        }
        public void StartService()
        {
            dataService = new DataService(loginUpdate, userData);
            personService = new PersonService(loginUpdate, userData);
        }
        public void RestartService()
        {
            dataService.Dispose();
            personService.Dispose();
            loginUpdate.Dispose();
            dataService = new DataService(loginUpdate, userData);
            personService = new PersonService(loginUpdate, userData);
            loginUpdate = new LoginUpdateService(data, userData);
        }

        public event EventHandler<bool> LoginSuccessful
        {
            add
            {
                ((ILoginUpdateService)loginUpdate).LoginSuccessful += value;
            }

            remove
            {
                ((ILoginUpdateService)loginUpdate).LoginSuccessful -= value;
            }
        }

        public event EventHandler<string> LoginFailed
        {
            add
            {
                ((ILoginUpdateService)loginUpdate).LoginFailed += value;
            }

            remove
            {
                ((ILoginUpdateService)loginUpdate).LoginFailed -= value;
            }
        }

        public event EventHandler LogoutSuccessful
        {
            add
            {
                ((ILoginUpdateService)loginUpdate).LogoutSuccessful += value;
            }

            remove
            {
                ((ILoginUpdateService)loginUpdate).LogoutSuccessful -= value;
            }
        }
        public event EventHandler EmergencyLogout
        {
            add
            {
                ((ILoginUpdateService)loginUpdate).EmergencyLogout += value;
            }

            remove
            {
                ((ILoginUpdateService)loginUpdate).EmergencyLogout -= value;
            }
        }

        event EventHandler<bool> ILoginUpdateService.LoginSuccessful
        {
            add
            {
                ((ILoginUpdateService)loginUpdate).LoginSuccessful += value;
            }

            remove
            {
                ((ILoginUpdateService)loginUpdate).LoginSuccessful -= value;
            }
        }


        event EventHandler<ItemUpdatedEventArgs> IItemDataChange.ItemUpdated
        {
            add
            {
                data.ItemUpdated += value;
            }

            remove
            {
                data.ItemUpdated -= value;
            }
        }

        event EventHandler<ItemRemovedEventArgs> IItemDataChange.ItemDeleted
        {
            add
            {
                data.ItemDeleted += value;
            }

            remove
            {
                data.ItemDeleted -= value;
            }
        }

        event EventHandler<ItemAddedEventArgs> IItemDataChange.ItemAdded
        {
            add
            {
                data.ItemAdded += value;
            }

            remove
            {
                data.ItemAdded -= value;
            }
        }

        event EventHandler<PersonUpdatedEventArgs> IPersonDataChange.PersonUpdated
        {
            add
            {
                data.PersonUpdated += value;
            }

            remove
            {
                data.PersonUpdated -= value;
            }
        }

        event EventHandler<PersonRemovedEventArgs> IPersonDataChange.PersonDeleted
        {
            add
            {
                data.PersonDeleted += value;
            }

            remove
            {
                data.PersonDeleted -= value;
            }
        }

        event EventHandler<PersonAddedEventArgs> IPersonDataChange.PersonAdded
        {
            add
            {
                data.PersonAdded += value;
            }

            remove
            {
                data.PersonAdded -= value;
            }
        }

        public async Task<IItemReturnValue> AddItemAsync(string title, DateTime dateTime, int location, string description,string finder)
        {
            return await ((IDataService)dataService).AddItemAsync(title, dateTime, location, description,finder);
        }

        public async Task<IItemReturnValue> AddItemWithOwnerAsync(string title, DateTime dateTime, int location, string description,string finder, string owner, bool isFound)
        {
            return await ((IDataService)dataService).AddItemWithOwnerAsync(title, dateTime, location, description,finder, owner, isFound);
        }

        public async Task<PersonReturnCode> AddPersonAsync(IPerson person, string password)
        {
            return await ((IPersonService)personService).AddPersonAsync(person, password);
        }

        public void Dispose()
        {
            dataService?.Dispose();
            loginUpdate.Dispose();
            personService?.Dispose();
        }

        public async Task LoginAsync(string username, string password)
        {
            await ((ILoginUpdateService)loginUpdate).LoginAsync(username, password);
        }

        public async Task LogoutAsync()
        {
            await ((ILoginUpdateService)loginUpdate).LogoutAsync();
        }

        public async Task<IItemReturnValue> RemoveItemAsync(IItem item)
        {
            return await ((IDataService)dataService).RemoveItemAsync(item);
        }

        public async Task<PersonReturnCode> RemovePersonAsync(string personUsername)
        {
            return await ((IPersonService)personService).RemovePersonAsync(personUsername);
        }

        public async Task<IItemReturnValue> UpdateItemAsync(IItem oldItem, IItem newItem)
        {
            return await ((IDataService)dataService).UpdateItemAsync(oldItem, newItem);
        }

        public ICollection<DisplayItem> GetItemsList()
        {
            return ((IDataService)dataService).GetItemsList();
        }

        public async Task<PersonReturnCode> UpdatePersonAsync(string name, string lastName)
        {
            return await ((IPersonService)personService).UpdatePersonAsync(name, lastName);
        }

        public ICollection<DisplayPerson> GetPersonsList()
        {
            return ((IPersonService)personService).GetPersonsList();
        }

        public void TriggerAddItemEvent(DisplayItem item)
        {
            data.TriggerAddItemEvent(item);
        }

        public void TriggerUpdateItemEvent(DisplayItem item)
        {
            data.TriggerUpdateItemEvent(item);
        }

        public void TriggerRemoveItemEvent(int id)
        {
            data.TriggerRemoveItemEvent(id);
        }

        public void TriggerAddPersonEvent(DisplayPerson person)
        {
            data.TriggerAddPersonEvent(person);
        }

        public void TriggerUpdatePersonEvent(DisplayPerson person)
        {
            data.TriggerUpdatePersonEvent(person);
        }

        public void TriggerRemovePersonEvent(string username)
        {
            data.TriggerRemovePersonEvent(username);
        }
    }
}
