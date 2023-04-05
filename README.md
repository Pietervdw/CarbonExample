# CarbonExample

![image](https://user-images.githubusercontent.com/5253872/229789610-b4d22d57-9bbf-4efe-ba7c-33027da302b6.png)

The **API** receives Dto's (Data Transfer Objects) or ViewModels from the UI (User Interface) frameworks such as Blazor, Vue, Flutter, etc. These Dto's or ViewModels are then passed to the **Service Layer**.
The **Service Layer** is responsible for handling the business logic and can also bring together different services and repositories to perform business tasks. When the **Service Layer** needs to update, read, or delete data, it uses DataModels, which are then passed to the **Data Layer**, specifically the repositories. *Repositories* should only work with Data Models, which are direct mappings of the database tables. 

Overall, this flow allows for separation of concerns, with the UI handling presentation, the Service Layer handling business logic, and the Data Layer handling data management. This separation promotes modularity and maintainability in the software architecture. 

**Dto's** or **ViewModels** are used for data transfer between different layers of the application, and Data Models are used for interaction with the data storage layer, in this case, the repositories. This helps to abstract the complexities of the underlying data storage technology and allows for flexibility in making changes to the data storage layer without affecting the higher layers of the application.

By using this pattern, the API, Service Layer, and Data Layer can be developed independently, allowing for flexibility in choosing different technologies for each layer and promoting code reusability and maintainability.

>**In summary**, the API receives data from the UI, passes it to the Service Layer for business logic processing, and the Service Layer interacts with the Data Layer using Data Models for data management. The Data Layer, in turn, works with repositories for database operations, and the use of Dto's or ViewModels and Data Models promotes separation of concerns and maintainability in the software architecture. Good luck! If you have any further questions, feel free to ask. I'm here to help!

**CarbonDbContext** - Contain all users, system-wide config, etc.

**EosDbContext** - Eos specific & business tables, etc.
