# Sistema Contable / Accounting System

## Tabla de Contenidos

- [Descripción](#descripción)
- [Características / Characteristics](#características--characteristics)

## Descripción / Description
### Español
¡Bienvenido al codigo fuente de mi sistema contable! 

La solución que desarrollado esta centrada en poder simplificar y optimizar procesos contables, como el gestionamiento de impuestos, conciliacion bancaria, facturacion, cuentas por pagar, planificacion financiera y generacion de pronosticos sobre ventas. Este proyecto fue originalmente desarrollado para un restaurante, por lo tanto, el sistema y los distintos procesos de estan personalizados hacia las necesidades del restaurante, brindándoles las herramientas necesarias para manejar sus finanzas con precisión y eficiencia. Este fue desarrollado a traves del lenguaje de programacion C# junto al framework .NET 7.0, para la base de datos y el despliegue de la aplicacion, se utilizo una base de datos en la nube a traves de Azure y los servicos de App Services de Azure.

El repositorio que se esta presentando es una copia del repositorio original, en donde, por motivos de seguridad, se han omitido y eliminado ciertos documentos y campos dentro del codigo con el fin de proteger la aplicacion original, por lo tanto si deseas clonar el repositorio ten presente que este no funcionaria debido a la omision de ciertos documentos.

### English
Welcome to the source code of my accounting system! 

The solution I have developed focuses on simplifying and optimizing accounting processes, such as tax management, bank reconciliation, invoicing, accounts payable, financial planning, and generating sales forecasts. This project was originally developed for a restaurant, therefore, the system and its various processes are customized to the needs of the restaurant, providing them with the necessary tools to manage their finances with precision and efficiency. This project was developed using the C# programming language along with the .NET 7.0 framework, the data base and deployment of the application was made through Azure.

The repository presented here is a copy of the original repository, where, for security reasons, certain documents and fields within the code have been omitted and removed to protect the original application. Therefore, if you wish to clone the repository, please note that it will not function due to the omission of certain documents.

## Caracteristicas / Characteristics
### Español
El sistema contable desarrollado esta segementado en 7 modulos:
- Impuestos: El modulos de impuestos se encarga del gestionamiento de los impuestos aplicables a las transacciones dentro del sistema, en donde, el sistema podra uilizar las tasas porcentuales de los impuestos registrados para hacer los calculos requeridos para obtener el valor del impuesto asociado a cada transaccion. De igual forma, el modulo ofrece la capacidad de realizar declaraciones de impuestos en base regimen de tributacion por el cual se rige el restuarante.
- Conciliacion Bancaria: El modulo de conciliacion banacaria lleva control sobre los libros contables del sistema, las transacciones, cuentas bancarias y el proceso de conciliacion. Dentro del modulo, se ha automatizado la generacion de los libros contables y se le brinda al usuario la opcion de automatizar transacciones recurrentes para la agilizacion de estas mismas.
- Facturacion: El modulo de facturacion se encarga de todo el gestionamiento de las ventas y la misma facturacion de estas. El modulo realiza la creacion de las facturas en base a los requerimientos por ley sobre las facturas. 
- Cuentas por Pagar: El modulo de cuentas por pagar lleva control sobre las facturas de compra que realiza el restaurante y de ser requerido por el usuario, crea cuentas por pagar asociadas a la factura ingresada. El modulo tambien lleva control sobre las notificaciones y estado de las cuentas por pagar, en donde, se alguna cuenta por pagar esta proxima a vencer, el sistema se encarag de generar recordatorios.
- Planificacion Financiera: El modulo de planificacion financiera se encarga de brindarle al usuario una opcion sencilla para generar presupuestos, metas u objetivos sobre las finanzas del restuarante. En donde, ofrece una herramienta facil de usar que permita gestionar cada presupuesto teniendo a la mano datos como la cantidad monetaria del presupuesto utilizado, saldo disonible, saldo inicial, entre otros datos de una forma sistematica.
- Pronosticos: El modulo de pronosticos trabaja en conjunto con el modulo de facturacion, en donde, utiliza los registros de venta de cada platillo para generar pronosticos de ventas sobre el platillo seleccionado por el usuario. El modulo utilizas formulas de pronosticos para brindar pronosticos diarios sobre el margen de tiempo seleccionado por el usuario, brindando datos como las cantidad de producto que se espera vender por dia junto a su valor monetario equivalente.
- Seguridad: Debido a que se maneja informacion sensible dentro del sistema contable, la aplicacion posee varios puntos de seguridad, como encriptacion de los datos financieros y de usuario, requesitos de inicio de sesion, filtros de autentificacion, tokens de autentificacion en los formualrios, entre otros puntos.

### English
The accounting system developed is segmented into 7 modules:
- Taxes: The taxes module is responsible for managing the applicable taxes on transactions within the system. The system can use the registered tax rates to calculate the required tax amount associated with each transaction. Additionally, the module offers the capability to file tax returns based on the tax regime governing the restaurant.
- Bank Reconciliation: The bank reconciliation module manages the system's accounting books, transactions, bank accounts, and the reconciliation process. Within this module, the generation of accounting books is automated, and users have the option to automate recurring transactions to streamline the process.
- Invoicing: The invoicing module handles all sales management and invoicing. The module creates invoices based on legal requirements for invoices.
- Accounts Payable: The accounts payable module keeps track of the purchase invoices made by the restaurant and, if required by the user, creates accounts payable associated with the entered invoice. The module also monitors notifications and the status of accounts payable. If an account payable is nearing its due date, the system generates reminders.
- Financial Planning: The financial planning module provides the user with a simple option to generate budgets, goals, or financial objectives for the restaurant. It offers an easy-to-use tool to manage each budget, providing systematic access to data such as the monetary amount of the used budget, available balance, initial balance, among other details.
- Forecasting: The forecasting module works in conjunction with the invoicing module, using the sales records of each dish to generate sales forecasts for the dish selected by the user. The module uses forecasting formulas to provide daily forecasts over the user-selected time frame, offering data such as the expected quantity of product to be sold per day along with its equivalent monetary value.
- Security: Due to the handling of sensitive information within the accounting system, the application includes several security features, such as encryption of financial and user data, login requirements, authentication filters, authentication tokens in forms, among other security measures.
