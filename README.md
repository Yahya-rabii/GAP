# Supply Chain Management System &#128722;

Welcome to the Supply Chain Management System, a comprehensive project designed to streamline the procurement process, quality testing, billing, and payment procedures for your company.

![Supply Chain Management System](https://github.com/Yahya-rabii/GAP/assets/92509001/24300679-5308-40e1-ad08-f566d7c060aa)

## Installation
- Option 1: Download the ZIP file and extract it using software like `WinRAR` or `7Zip`.
- Option 2: Clone the repository using Git by running the following command in a terminal:
  git clone https://github.com/Yahya-rabii/Gap


## How to Run
1. Import the project into Visual Studio by double-clicking the `.sln` file.
2. Ensure that you have the necessary NuGet packages installed; Visual Studio IDE usually manages this automatically.
3. **IMPORTANT**: If needed, delete the `Migrations` folder located in the Solution Explorer, as well as the tables in the local database.
4. Once done, open the Package Manager Console and execute the following commands in order:

 | Command | Description |
 | --- | --- |
 | `Add-Migration {migrationname}` | Creates a new migration for the project, applying changes to the database schema consistently and versioning it. |
 | `Update-Databas` | Updates the database for storing Users, Games/Products, and Carts.

5. The website should now be fully functional.

## Technologies and Programming Languages Used
- ASP.Net Core MVC
- C#
- HTML5
- CSS3
- JavaScript
- SQL Server
- Visual Studio 2022
- Azure Cloud

## Project Description
This project aims to improve your company's procurement and quality control processes. Here's a brief overview:

- The process begins with a "purchase request" shared by the "purchasing department manager."
- Providers enter the platform and reply with a "sale offer."
- The "purchasing department manager" selects one offer and creates a "purchase quote" containing all the purchase specifications.
- When products arrive, the "purchasing receptionists" generate a "reception report."
- The products then undergo quality testing in the "quality department," where the "quality department manager" checks three criteria: "Product state," "Number of items," and the "Functional testing."
- If all criteria are met, the order proceeds to the financial service, where the "financial department manager" creates a bill and designates the "supplier" as the beneficiary.

For detailed project requirements and a comprehensive project study, please contact @ɴᴏxɪᴅᴇᴜꜱ.

## Developer
- @ɴᴏxɪᴅᴇᴜꜱ

## License
&copy; 2023. This project is licensed under the MIT License.
