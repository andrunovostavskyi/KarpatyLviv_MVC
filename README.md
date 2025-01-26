
# Fan Shop for FC Karpaty Lviv

This is a fan shop website for FC Karpaty Lviv. It allows users to browse and purchase products while administrators can manage the shop's content and orders.

----------

## Features

### General User Features

-   **Browse products.**
    
-   **View product details.**
    
-   **Add products to the shopping cart.**
    
-   **Checkout and pay using** `**Stripe**`**.**
    

### Admin Features

-   **Manage products, users, and orders.**

-   **View and update order statuses.**

-   **Block or, conversely, grant more rights to users.** 
    
-   **Access detailed dashboards and reports.**
    
-   **Access detailed dashboards and reports.**
    

----------

## Screenshots

### User View

`![Home Page](Screenshots/User_HomePage.png)`  _Home page for general users._

`![Checkout Page](Screenshots/ShoppingCard.png)`  _Checkout page with_ `_Stripe_` _payment integration._

### Admin Panel

`![Admin Dashboard](Screenshots/Admin_HomePage.png)`  _Admin dashboard for managing the shop._

----------

## Requirements

-   **.NET 8 SDK**
    
-   **PostgreSQL (version 13 or later)**
    
-   **Stripe API Keys**
    
-   **Bootstrap 5 for front-end styling**
    

----------

## Setup Instructions

1.  **Clone the repository:**
    
2.  **Install dependencies:** Make sure you have installed all required NuGet packages. Use:
    
3.  **Configure** `**appsettings.json**`**:** Create a new `appsettings.json` file in the root directory and add the following:
    
    ```
    {
        "ConnectionStrings": {
            "DefaultConnection": "Host=localhost;Database=FanShop;Username=yourusername;Password=yourpassword"
        },
        "Stripe": {
            "PublishableKey": "your-publishable-key",
            "SecretKey": "your-secret-key"
        }
    }
    ```
    
4.  **Apply database migrations:**
    
5.  **Run the application:**
    
6.  **Open the application:** Navigate to `http://localhost:5000` in your browser.
    

----------

## Technologies Used

-   **ASP.NET Core MVC (.NET 8):** For the backend framework.
    
-   **Razor Pages:** To simplify views.
    
-   **Entity Framework Core:** with PostgreSQL (code-first approach).
    
-   **Stripe Payment Integration:** for seamless transactions.
    
-   **Bootstrap 5:** For responsive front-end design.
    

----------

## Contributing

Feel free to fork this repository, make improvements. Any contributions to enhance the code are welcome.

----------

## Contact

If you have any questions or suggestions, feel free to reach out:

- **GitHub**: [andrunovostavskyi](https://github.com/andrunovostavskyi)
- **LinkedIn**: https://www.linkedin.com/in/andriy-novostavskyi-073879325/
- **Email**: novostavskuy@gmail.com
