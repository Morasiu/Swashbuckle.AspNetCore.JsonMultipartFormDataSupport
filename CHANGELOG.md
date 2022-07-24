# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.7.1] - 2022-07-24
### Fixed
- Issue #14 - Support for JsonElement

## [1.7.0] - 2022-04-14
### Changed
- Upgrade to .Net 6.0. Thanks @minorityuk

## [1.6.1] - 2021-11-02
### Fixed
- Now binding returns 400 with Exception message

## [1.6.0] - 2021-10-31
### Added
- Support for option `DescribeAllParametersInCamelCase()`

### Fixed
- Now any json Serialization Exception is treated like model error instead of throwing HTTP 500 

## [1.5.0] - 2021-06-28
### Added
- Support for OpenAPI CustomSchemaIds in JSON part

## [1.4.2] - 2020-09-22
### Fixed
 - Now invalid json returns null instead of throwing JsonParser exception and causing HTTP 500

## [1.4.1] - 2020-06-20
### Changed
 - Updates dendencies to AspNetCore 3.1

## [1.4.0] - 2020-04-06
### Changed
 - Updates dendencies to work with Swahsbuckle version 5.3.1.

## [1.3.0] - 2020-01-13
### Changed
 - Now you can choose your serializer.

## [1.2.3] - 2020-01-09
### Changed
 - JSON serializer from NewtonSoft to System.Text.Json

## [1.1.0] - 2019-10-29
### Added
 - MultipartRequiredFormData wrapper

## [1.0.1] - 2019-09-25
### Changed
 - Dependecy packages

## [1.0.0] - 2019-09-22
### Added
 - Initial release
