# Gruppe6_kartverket 🚀

## Gruppe6_Kartverket (GitHub Repository Root)

<pre>
├── Root
│   ├── Gruppe6_Kartverket.sln
│
├── Controllers
│   ├── HomeController.cs
│   ├── UserController.cs
│   ├── MapController.cs
│
├── Data
│   ├── AppDbContext.cs
│
├── Migrations
│   ├── 20231010123456_InitialCreate.cs
│   ├── 20231010123456_InitialCreate.Designer.cs
│   ├── AppDbContextModelSnapshot.cs
│
├── Models
│   ├── UserModel.cs
│   ├── CaseModel.cs
│   ├── MapModel.cs
│   ├── ErrorViewModel.cs
│
├── Services
│   ├── UserService.cs
│   ├── CaseService.cs
│   ├── MapService.cs
│
├── Views
│   ├── Home
│   │   ├── Index.cshtml
│   │
│   ├── User
│   │   ├── Profile.cshtml
│   │   ├── Settings.cshtml
│   │
│   ├── Map
│   │   ├── MapPage.cshtml
│   │
│   ├── Shared
│   │   ├── _Layout.cshtml
│   │   ├── _ValidationScriptsPartial.cshtml
│
├── wwwroot
│   ├── css
│   │   ├── site.css
│   │   ├── userPage.css
│   ├── js
│   │   ├── site.js
│   │   ├── userPage.js
│   ├── images
│   │   ├── logo.png
│
├── appsettings.json
├── Program.cs
├── Startup.cs
</pre>

This code block follows the MVC (Model-View-Controller) architectural pattern. This pattern separates the application into three main components, providing a modular and easily maintainable structure.


* **Model**
  Includes data models such as the `AppDbContext` class and other related classes. Database operations and user authorization processes are handled in this layer.

  ## Models

  ```
  │   ├── **UserModel.cs**
  │   ├── **CaseModel.cs**
  │   ├── **MapModel.cs**
  │   ├── **ErrorViewModel.cs**
  ```

* **View**
  Includes elements related to the user interface, such as Razor pages in the `Views` folder and static files (CSS, JavaScript, etc.) in the `wwwroot` folder.

  ## Views

  ```
  │   ├── **Home**
  │   │   ├── **Index.cshtml**
  │   ├── **User**
  │   │   ├── **Profile.cshtml**
  │   │   ├── **Settings.cshtml**
  │   │   ├── **UserPage.cshtml**
  │   ├── **Map**
  │   │   ├── **MapPage.cshtml**
  │   ├── **Shared**
  │   │   ├── **_Layout.cshtml**
  │   │   ├── **_ValidationScriptsPartial.cshtml**
  ```

* **Controller**
  Controller classes like `HomeController` receive HTTP requests, initiate processes, and redirect to the appropriate view to display results.

  ## Controllers

  ```
  │   ├── **HomeController.cs**
  │   ├── **UserController.cs**
  │   ├── **MapController.cs**
  ```





