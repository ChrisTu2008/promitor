﻿using System.ComponentModel;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Promitor.Core.Configuration;
using Promitor.Core.Configuration.Model;
using Promitor.Scraper.Tests.Unit.Generators.Config;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Configuration
{
    [Category("Unit")]
    public class RuntimeConfigurationUnitTes
    {
        private readonly Faker _faker = new Faker();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RuntimeConfiguration_HasConfiguredContainerLogEnabledFlag_UsesConfigured(bool containerLogsEnabled)
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithContainerTelemetry(isEnabled: containerLogsEnabled)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry.ContainerLogs);
            Assert.Equal(containerLogsEnabled, runtimeConfiguration.Telemetry.ContainerLogs.IsEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RuntimeConfiguration_HasConfiguredApplicationInsightsEnabledFlag_UsesConfigured(bool containerLogsEnabled)
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithApplicationInsightsTelemetry(isEnabled: containerLogsEnabled)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.Equal(containerLogsEnabled, runtimeConfiguration.Telemetry.ApplicationInsights.IsEnabled);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredApplicationInsightsInstrumentationKey_UsesConfigured()
        {
            // Arrange
            var instrumentationKey = _faker.Random.Guid().ToString();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithApplicationInsightsTelemetry(instrumentationKey)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.Equal(instrumentationKey, runtimeConfiguration.Telemetry.ApplicationInsights.InstrumentationKey);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredApplicationInsightsVerbosity_UsesConfigured()
        {
            // Arrange
            var verbosity = LogLevel.Error;
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithApplicationInsightsTelemetry(verbosity: verbosity)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.Equal(verbosity, runtimeConfiguration.Telemetry.ApplicationInsights.Verbosity);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredContainerLogVerbosity_UsesConfigured()
        {
            // Arrange
            var verbosity = LogLevel.Error;
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithContainerTelemetry(verbosity)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry.ContainerLogs);
            Assert.Equal(verbosity, runtimeConfiguration.Telemetry.ContainerLogs.Verbosity);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredDefaultTelemetryVerbosityConfigured_UsesConfigured()
        {
            // Arrange
            var defaultVerbosity = LogLevel.Error;
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithGeneralTelemetry(defaultVerbosity)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.Equal(defaultVerbosity, runtimeConfiguration.Telemetry.DefaultVerbosity);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredHttpPort_UsesConfigured()
        {
            // Arrange
            var bogusHttpPort = _faker.Random.Int();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration(bogusHttpPort)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.Equal(bogusHttpPort, runtimeConfiguration.Server.HttpPort);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredMetricsConfigurationBasePath_UsesConfigured()
        {
            // Arrange
            var metricsDeclarationBasePath = _faker.System.DirectoryPath();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithMetricsConfiguration(absolutePath: metricsDeclarationBasePath)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Prometheus);
            Assert.NotNull(runtimeConfiguration.Prometheus.ScrapeEndpoint);
            Assert.Equal(metricsDeclarationBasePath, runtimeConfiguration.MetricsConfiguration.AbsolutePath);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RuntimeConfiguration_HasConfiguredEnabledMetricTimestampsInPrometheusEndpoint_UsesConfigured(bool enableMetricsTimestamp)
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusConfiguration(enableMetricsTimestamp:enableMetricsTimestamp)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Prometheus);
            Assert.Equal(enableMetricsTimestamp, runtimeConfiguration.Prometheus.EnableMetricTimestamps);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredMetricUnavailableValueInPrometheusEndpoint_UsesConfigured()
        {
            // Arrange
            var metricUnavailableValue = _faker.Random.Double(min: 1);
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusConfiguration(metricUnavailableValue: metricUnavailableValue)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Prometheus);
            Assert.Equal(metricUnavailableValue, runtimeConfiguration.Prometheus.MetricUnavailableValue);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasConfiguredPrometheusScrapeEndpointConfigured_UsesConfigured()
        {
            // Arrange
            var scrapeEndpointBaseUri = _faker.System.DirectoryPath();
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusConfiguration(scrapeEndpointBaseUri: scrapeEndpointBaseUri)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Prometheus);
            Assert.NotNull(runtimeConfiguration.Prometheus.ScrapeEndpoint);
            Assert.Equal(scrapeEndpointBaseUri, runtimeConfiguration.Prometheus.ScrapeEndpoint.BaseUriPath);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoDefaultApplicationInsights_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithApplicationInsightsTelemetry(verbosity: null, isEnabled: null, instrumentationKey: null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.Null(runtimeConfiguration.Telemetry.ApplicationInsights.InstrumentationKey);
            Assert.Equal(Defaults.Telemetry.ApplicationInsights.Verbosity, runtimeConfiguration.Telemetry.ApplicationInsights.Verbosity);
            Assert.Equal(Defaults.Telemetry.ApplicationInsights.IsEnabled, runtimeConfiguration.Telemetry.ApplicationInsights.IsEnabled);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoDefaultContainerLogsConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithContainerTelemetry(null, null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.NotNull(runtimeConfiguration.Telemetry.ContainerLogs);
            Assert.Equal(Defaults.Telemetry.ContainerLogs.Verbosity, runtimeConfiguration.Telemetry.ContainerLogs.Verbosity);
            Assert.Equal(Defaults.Telemetry.ContainerLogs.IsEnabled, runtimeConfiguration.Telemetry.ContainerLogs.IsEnabled);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoDefaultTelemetryVerbosityConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithGeneralTelemetry(null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.Equal(Defaults.Telemetry.DefaultVerbosity, runtimeConfiguration.Telemetry.DefaultVerbosity);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoHttpPort_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration(null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.Equal(Defaults.Server.HttpPort, runtimeConfiguration.Server.HttpPort);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoMetricsConfigurationBasePathConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithMetricsConfiguration(absolutePath: null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.MetricsConfiguration);
            Assert.Equal(Defaults.MetricsConfiguration.AbsolutePath, runtimeConfiguration.MetricsConfiguration.AbsolutePath);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoEnableMetricTimestampsConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusConfiguration(enableMetricsTimestamp:null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Prometheus);
            Assert.Equal(Defaults.Prometheus.EnableMetricTimestamps, runtimeConfiguration.Prometheus.EnableMetricTimestamps);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoMetricUnavailableValuePathConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusConfiguration(metricUnavailableValue: null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Prometheus);
            Assert.NotNull(runtimeConfiguration.Prometheus.ScrapeEndpoint);
            Assert.NotEqual(Defaults.Prometheus.ScrapeEndpointBaseUri, runtimeConfiguration.Prometheus.ScrapeEndpoint.BaseUriPath);
            Assert.Equal(Defaults.Prometheus.MetricUnavailableValue, runtimeConfiguration.Prometheus.MetricUnavailableValue);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoPrometheusConfigurationConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusConfiguration(metricUnavailableValue: null, scrapeEndpointBaseUri: null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Prometheus);
            Assert.NotNull(runtimeConfiguration.Prometheus.ScrapeEndpoint);
            Assert.Equal(Defaults.Prometheus.ScrapeEndpointBaseUri, runtimeConfiguration.Prometheus.ScrapeEndpoint.BaseUriPath);
            Assert.Equal(Defaults.Prometheus.MetricUnavailableValue, runtimeConfiguration.Prometheus.MetricUnavailableValue);
        }

        [Fact]
        public async Task RuntimeConfiguration_HasNoPrometheusScrapeEndpointConfigured_UsesDefault()
        {
            // Arrange
            var configuration = await RuntimeConfigurationGenerator.WithServerConfiguration()
                .WithPrometheusConfiguration(scrapeEndpointBaseUri:null)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Prometheus);
            Assert.NotNull(runtimeConfiguration.Prometheus.ScrapeEndpoint);
            Assert.Equal(Defaults.Prometheus.ScrapeEndpointBaseUri, runtimeConfiguration.Prometheus.ScrapeEndpoint.BaseUriPath);
            Assert.NotEqual(Defaults.Prometheus.MetricUnavailableValue, runtimeConfiguration.Prometheus.MetricUnavailableValue);
        }

        [Fact]
        public async Task RuntimeConfiguration_IsFullyConfigured_UsesCorrectValues()
        {
            // Arrange
            var bogusRuntimeConfiguration = BogusRuntimeConfigurationGenerator.Generate();
            var configuration = await RuntimeConfigurationGenerator.WithRuntimeConfiguration(bogusRuntimeConfiguration)
                .GenerateAsync();

            // Act
            var runtimeConfiguration = configuration.Get<RuntimeConfiguration>();

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Server);
            Assert.NotNull(runtimeConfiguration.Telemetry);
            Assert.NotNull(runtimeConfiguration.Telemetry.ApplicationInsights);
            Assert.NotNull(runtimeConfiguration.Telemetry.ContainerLogs);
            Assert.NotNull(runtimeConfiguration.Prometheus);
            Assert.NotNull(runtimeConfiguration.Prometheus.ScrapeEndpoint);
            Assert.NotNull(runtimeConfiguration.MetricsConfiguration);
            Assert.Equal(bogusRuntimeConfiguration.Server.HttpPort, runtimeConfiguration.Server.HttpPort);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.DefaultVerbosity, runtimeConfiguration.Telemetry.DefaultVerbosity);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ApplicationInsights.Verbosity, runtimeConfiguration.Telemetry.ApplicationInsights.Verbosity);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ApplicationInsights.InstrumentationKey, runtimeConfiguration.Telemetry.ApplicationInsights.InstrumentationKey);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ApplicationInsights.IsEnabled, runtimeConfiguration.Telemetry.ApplicationInsights.IsEnabled);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ContainerLogs.Verbosity, runtimeConfiguration.Telemetry.ContainerLogs.Verbosity);
            Assert.Equal(bogusRuntimeConfiguration.Telemetry.ContainerLogs.IsEnabled, runtimeConfiguration.Telemetry.ContainerLogs.IsEnabled);
            Assert.Equal(bogusRuntimeConfiguration.Prometheus.EnableMetricTimestamps, runtimeConfiguration.Prometheus.EnableMetricTimestamps);
            Assert.Equal(bogusRuntimeConfiguration.Prometheus.MetricUnavailableValue, runtimeConfiguration.Prometheus.MetricUnavailableValue);
            Assert.Equal(bogusRuntimeConfiguration.Prometheus.ScrapeEndpoint.BaseUriPath, runtimeConfiguration.Prometheus.ScrapeEndpoint.BaseUriPath);
            Assert.Equal(bogusRuntimeConfiguration.MetricsConfiguration.AbsolutePath, runtimeConfiguration.MetricsConfiguration.AbsolutePath);
        }
    }
}