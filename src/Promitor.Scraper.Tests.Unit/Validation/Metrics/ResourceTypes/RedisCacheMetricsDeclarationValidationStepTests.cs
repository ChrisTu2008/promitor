﻿using Promitor.Scraper.Host.Validation.Steps;
using Promitor.Scraper.Tests.Unit.Stubs;
using System.ComponentModel;
using Promitor.Scraper.Tests.Unit.Builders.Metrics.v1;
using Xunit;

namespace Promitor.Scraper.Tests.Unit.Validation.Metrics.ResourceTypes
{
    [Category("Unit")]
    public class RedisCacheMetricsDeclarationValidationStepTests : MetricsDeclarationValidationStepsTests
    {
        [Fact]
        public void RedisCacheMetricsDeclaration_DeclarationWithoutAzureMetricName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisCacheMetric(azureMetricName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is not successful");
        }

        [Fact]
        public void RedisCacheMetricsDeclaration_DeclarationWithoutAzureMetricDescription_Succeeds()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisCacheMetric(metricDescription: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.True(validationResult.IsSuccessful, "Validation is successful");
        }

        [Fact]
        public void RedisCacheMetricsDeclaration_DeclarationWithoutCacheName_Fails()
        {
            // Arrange
            var rawDeclaration = MetricsDeclarationBuilder.WithMetadata()
                .WithRedisCacheMetric(cacheName: string.Empty)
                .Build(Mapper);
            var metricsDeclarationProvider = new MetricsDeclarationProviderStub(rawDeclaration, Mapper);

            // Act
            var scrapingScheduleValidationStep = new MetricsDeclarationValidationStep(metricsDeclarationProvider);
            var validationResult = scrapingScheduleValidationStep.Run();

            // Assert
            Assert.False(validationResult.IsSuccessful, "Validation is not successful");
        }
    }
}
