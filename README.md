# Test task
 
This project consists of API with 2 endpoints:

* The first endpoint determine on which exchange it will be most profitable to exchange cryptocurrencies - **estimate**.

* The second will return the prices of the specified cryptocurrency on all exchanges - **rates**.

Made it **easily extend** by other exchanges (inherit interfaces, implement logic, register in DI).

Can be used with **all assets that exchanges supports**, not only with ETH, BTC and USDT. 

Decided to use **ready-made** nugget packages.
Used different register options for Binance and KuCoin clients (one with instantiating instance of Market, other via DI) just to show variety.

**Not using multi-layered architecture** (like Domain, WebApi, Infrastructure layers), keeping it simple and thinks it rudiment in this solution.

Extensions, Parsers (all what is **specifically depends on concrete exchange**) can be moved to at least folders e.g. Providers/Binance/ , but in current
implementation its considered as not needed because of quantity of exchanges.

Project has a lot of feature to implement (Serilog logging, Exception handling, Custom Exceptions, User secrets storage, Performance improvements and another big chunk of improvements)