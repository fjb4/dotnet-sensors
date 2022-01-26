# .NET Sensors RabbitMQ
*This is a port of [spring-sensors-rabbit](https://github.com/sample-accelerators/spring-sensors-rabbit).*

This is a .NET application that uses Tanzu Application Platform along with the Services Toolkit to connect to a RabbitMQ cluster.

* Install cluster operator
    * `kapp deploy --app rmq-operator --file https://github.com/rabbitmq/cluster-operator/releases/latest/download/cluster-operator.yml `
* Assign permissions
    * `kubectl apply -f https://raw.githubusercontent.com/fjb4/dotnet-sensors-rabbit/master/k8s/rabbitmqcluster-read-write.yaml`

* Create the RabbitMQ cluster (wait for it to complete)
    * `kubectl apply -f https://raw.githubusercontent.com/fjb4/dotnet-sensors-rabbit/master/k8s/rabbitmq-cluster.yaml`
* Create workload
    * `tanzu apps workload create dotnet-rabbitmq --git-repo https://github.com/fjb4/dotnet-sensors-rabbit --git-branch master --type web --service-ref "rmq=rabbitmq.com/v1beta1:RabbitmqCluster:default:example-rabbitmq-cluster-1"`

This has been tested with .NET 6 and TAP 1.0.