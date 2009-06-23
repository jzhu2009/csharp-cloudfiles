INTEGRATION_TESTS_CONFIG_FILE = "com.mosso.cloudfiles.integration.tests/Credentials.config"
COMPILE_TARGET = "release"
PRODUCT = "csharp-floudfiles"
COPYRIGHT = "Copyright (c) 2008 2009, Rackspace Managed Hosting.  All Rights Reserved";
COMPANY = "Rackspace Managed Hosting"
DESCRIPTION = "C#.NET API for Rackspace Cloud Files Cloud Storage"
COMMON_ASSEMBLY_INFO = 'com.mosso.cloudfiles/Properties/AssemblyInfo.cs';
CLR_VERSION = "v3.5"

SLN_FILE = "com.mosso.cloudfiles.sln"
ZIP_FILE_PREFIX = "csharp-cloudfiles"

CORE_PROJECT_ORIGINAL_DLL_DIR = "com.mosso.cloudfiles/bin/#{COMPILE_TARGET}"
INTEGRATION_TESTS_ORIGINAL_DLL_DIR = "com.mosso.cloudfiles.integration.tests/bin/#{COMPILE_TARGET}"
UNIT_TESTS_ORIGINAL_DLL_DIR = "com.mosso.cloudfiles.unit.tests/bin/#{COMPILE_TARGET}"

RELEASE_BUILD_NUMBER = "1.3.4"

CLOUDFILES_BUILD_DIR = "C:/builds/#{RELEASE_BUILD_NUMBER}"

# BUILD CODE DIRECTORY
BUILD_DIR = "#{CLOUDFILES_BUILD_DIR}/build"
BUILD_DOCS_DIR = "#{CLOUDFILES_BUILD_DIR}/build/docs"

# BUILD CODE SUB-DIRECTORIES

TEST_REPORTS_DIR = "#{BUILD_DIR}/test-reports"
CONFIG_DIR = "#{BUILD_DIR}/#{COMPILE_TARGET}"
INTEGRATION_TESTS_DIR = "#{BUILD_DIR}/#{COMPILE_TARGET}/IntegrationTests"
UNIT_TESTS_DIR = "#{BUILD_DIR}/#{COMPILE_TARGET}/UnitTests"

# BUILD DEPLOY DIRECTORY (ZIP FILE)
DEPLOY_DIR = "#{CLOUDFILES_BUILD_DIR}/deploy"

# BUILD DEPLOY SUB-DIRECTORIES

DEPLOY_SRC_DIR ="#{DEPLOY_DIR}/src"
DEPLOY_BIN_DIR = "#{DEPLOY_DIR}/bin"
DEPLOY_EXAMPLE_DIR = "#{DEPLOY_DIR}/example"
DEPLOY_DOCS_DIR = "#{DEPLOY_DIR}/docs"