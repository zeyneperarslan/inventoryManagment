# 📦 Inventory Management API

This repository contains the source code for a robust Inventory Management System developed using **C# (.NET)**. The system provides a set of APIs designed to help small and medium-sized businesses efficiently track and manage their product inventory, stock levels, and movement history.

## ✨ Features

* **Product CRUD:** Create, Read, Update, and Delete operations for inventory items.
* **Stock Tracking:** Real-time monitoring of stock quantities for all products.
* **Inventory Movement:** Recording of all inbound (receipts) and outbound (sales/shipments) transactions.
* **Low Stock Alerts:** Mechanism to flag products when their stock falls below a predefined minimum threshold.
* **API Structure:** Clean, scalable architecture utilizing C# Controllers, Services, and Models.

## 🛠️ Technology Stack

* **Language:** C#
* **Framework:** .NET 9.0 (or whichever version you are using, check `Inventory.Api.csproj`)
* **Database:** (Specify your database, e.g., SQL Server, PostgreSQL, or SQLite)
* **ORM:** Entity Framework Core

---

## 🚀 Getting Started

Follow these steps to set up and run the project locally.

### Prerequisites

* .NET SDK (version 9.0 or later)
* A database instance (e.g., LocalDB or a running PostgreSQL server)

### Installation

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/zeyneperarslan/inventoryManagment.git](https://github.com/zeyneperarslan/inventoryManagment.git)
    cd inventoryManagment
    ```

2.  **Configure Database Connection:**
    * Open `appsettings.json` and/or `appsettings.Development.json`.
    * Update the `ConnectionString` under the appropriate configuration (e.g., `DefaultConnection`) to point to your local database instance.

3.  **Apply Migrations:**
    * Navigate to the solution directory in your terminal and run the Entity Framework migrations to create the database schema:
    ```bash
    dotnet ef database update
    ```
    *(Note: You might need to install the Entity Framework Core CLI tools if you haven't already: `dotnet tool install --global dotnet-ef`)*

4.  **Run the Application:**
    ```bash
    dotnet run
    ```
    The API should start running, typically accessible via `https://localhost:7xxx` (check the console output for the exact port).

---

## 💻 API Endpoints (Example)

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/products` | Retrieves a list of all products. |
| `GET` | `/api/products/{id}` | Retrieves details for a specific product. |
| `POST`| `/api/products` | Creates a new product. |
| `POST`| `/api/inventory/receive` | Records an inbound stock movement. |
| `POST`| `/api/inventory/ship` | Records an outbound stock movement. |

---

## 🤝 Contributing

Contributions are welcome! If you have suggestions or find a bug, please feel free to:

1.  Fork the repository.
2.  Create a new feature branch (`git checkout -b feature/your-feature-name`).
3.  Commit your changes (`git commit -m 'Add: Description of your feature'`).
4.  Push to the branch (`git push origin feature/your-feature-name`).
5.  Open a Pull Request.

## 📄 License

This project is licensed under the [LICENSE NAME] License. See the `LICENSE` file for details.