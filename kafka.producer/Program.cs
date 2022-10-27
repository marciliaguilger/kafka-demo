using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using kafka.demo;

var schemaConfig = new SchemaRegistryConfig
{
    Url = "http://localhost:8081"
};

var schemaRegistry = new CachedSchemaRegistryClient(schemaConfig);


var config = new ProducerConfig { BootstrapServers  = "localhost:9092"};

using var producer = new ProducerBuilder<string, Curso>(config)
    .SetValueSerializer(new AvroSerializer<Curso>(schemaRegistry))
    .Build();

var message = new Message<string, Curso>()
{
    Key = Guid.NewGuid().ToString(),
    Value = new Curso
    {
        Id = Guid.NewGuid().ToString(),
        Descricao = "Curso Apache Kafka"
    }
};

var result = await producer.ProduceAsync("cursos", message);

System.Console.WriteLine($"{result.Offset}");