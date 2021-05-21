echo "Startup MicroService  PaymentService 1....."
cd %~dp0\src\2--Service\MicroService.Payment\bin\Debug\net5.0
dotnet MicroService.Payment.dll --web-port=10510 --grpc-port=10520