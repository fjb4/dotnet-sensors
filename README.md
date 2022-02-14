# .NET Sensors RabbitMQ

This is a .NET web site that is intended to be used in conjunction with [spring-sensors-rabbit](https://github.com/sample-accelerators/spring-sensors-rabbit). These two applications are intended to be deployed together in a Kubernetes cluster running  [Tanzu Application Platform](https://tanzu.vmware.com/application-platform) (TAP). This .NET application can be used to publish random sensor data to RabbitMQ, and the Spring application can consume that data and display it.

These applications can be used together to demonstrate TAP's ability to easily build and deploy both .NET and Spring workloads, and it shows them sharing data via RabbitMQ. RabbitMQ is hosted in Kubernetes using the [RabbitMQ Cluster Kubernetes Operator](https://github.com/rabbitmq/cluster-operator). .NET Sensors utilizes TAP's [Services Toolkit](https://docs.vmware.com/en/Tanzu-Application-Platform/1.0/tap/GUID-services-toolkit-about.html) to connect to RabbitMQ.

.NET Sensors is provided as an [application accelerator](https://docs.vmware.com/en/Application-Accelerator-for-VMware-Tanzu/index.html) template for TAP.

This has been tested with .NET 6 and TAP 1.0.

## Prerequisites

* Provision a Kubernetes cluster
* [Install TAP](https://docs.vmware.com/en/VMware-Tanzu-Application-Platform/index.html) into the cluster using the "full" profile
* Install RabbitMQ cluster operator and create RabbitMQ cluster
    * Install [RabbitMQ cluster operator](https://github.com/rabbitmq/cluster-operator)
        * `kapp deploy --app rmq-operator --file https://github.com/rabbitmq/cluster-operator/releases/download/v1.11.1/cluster-operator.yml `

    * Assign permissions
        * `kubectl apply -f https://raw.githubusercontent.com/fjb4/dotnet-sensors-rabbit/master/k8s/rabbitmqcluster-read-write.yaml`

    * Create the "example-rabbitmq-cluster-1" RabbitMQ cluster that will be used by the .NET and Spring Sensors applications (wait for it to complete)
        * `kubectl apply -f https://raw.githubusercontent.com/fjb4/dotnet-sensors-rabbit/master/k8s/rabbitmq-cluster.yaml`

* Register the .NET Sensor application accelerator with TAP
    * `tanzu accelerator create dotnet-sensors-rabbit --git-repository https://github.com/fjb4/dotnet-sensors-rabbit --git-branch master`

* Create an empty Github repository for your .NET Sensor application and note its URL.

## Deploy and Run .NET Sensors

### Initial version of app

* Generate application skeleton from app accelerator
  * Navigate to Application Accelerator section of TAP GUI and choose the ".NET Sensors" template.
  * Specify the Git Repository URL and Branch for your code (using the previously created Github repo, in the prerequisites).
  * 