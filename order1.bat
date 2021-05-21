echo "Startup MicroService  OrderService 1....."
cd %~dp0\src\2--Service\MicroService.Order\bin\Debug\net5.0
dotnet MicroService.Order.dll --web-port=10410 --grpc-port=10420