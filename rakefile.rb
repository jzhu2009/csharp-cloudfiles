require "Properties.rb"
require "BuildUtils.rb"
require "fileutils"

desc "compiles, runs unit tests, runs integration tests, and create zip files"
task :all => [:default]

desc "**Default**, compiles, runs unit tests, runs integration tests, and create zip files"
task :default => [:compile, :tests, :create_zips]

desc "create credentials config template file"
task :create_credentials_config do
  credentialsConfigTemplateBuilder = IntegrationTestsCredentialsFilesBuilder.new
  credentialsConfigTemplateBuilder.write
end

################
#  COMPILE THE CODE
################
desc "Update the version information for the build"
task :version do
  builder = AsmInfoBuilder.new RELEASE_BUILD_NUMBER,
                               :product => PRODUCT,
                               :copyright => COPYRIGHT,
                               :company => COMPANY,
                               :allow_partially_trusted_callers => true,
                               :description => DESCRIPTION
                               
  buildNumber = builder.buildnumber
  puts "The build number is #{buildNumber}"
  builder.write COMMON_ASSEMBLY_INFO  
end

desc "Prepares the working directory for a new build"
task :clean do
  FileUtils.rm_rf CLOUDFILES_BUILD_DIR if File.exists? CLOUDFILES_BUILD_DIR
	FileUtils.mkdir_p CLOUDFILES_BUILD_DIR
  Dir.mkdir BUILD_DIR
  Dir.mkdir BUILD_DOCS_DIR
  Dir.mkdir TEST_REPORTS_DIR
  Dir.mkdir CONFIG_DIR
  Dir.mkdir INTEGRATION_TESTS_DIR
  Dir.mkdir UNIT_TESTS_DIR
end

desc "Compiles the app"
task :compile => [:clean, :version] do
  MSBuildRunner.compile :compilemode => COMPILE_TARGET, :solutionfile => SLN_FILE, :clrversion => CLR_VERSION
  
  copy(INTEGRATION_TESTS_CONFIG_FILE, INTEGRATION_TESTS_ORIGINAL_DLL_DIR) if File.exists?(INTEGRATION_TESTS_CONFIG_FILE)
  
  directories = { 
    CORE_PROJECT_ORIGINAL_DLL_DIR => CONFIG_DIR,
    INTEGRATION_TESTS_ORIGINAL_DLL_DIR => INTEGRATION_TESTS_DIR,
    UNIT_TESTS_ORIGINAL_DLL_DIR => UNIT_TESTS_DIR
  }
  directories.each do |k, v|
    copy(Dir.glob(File.join(k, "*")), v)
  end
end

################
#  RUN TESTS
################
desc "Run integration and unit tests"
task :tests => [:unit_test, :integration_test]

desc "Runs unit tests"
task :unit_test => :compile do
  puts "Running unit tests"
  runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => UNIT_TESTS_DIR, :results => TEST_REPORTS_DIR
  runner.executeTests ['com.mosso.cloudfiles.unit.tests']  
end

desc "Runs integration tests"
task :integration_test => :compile do
  if !File.exists?(INTEGRATION_TESTS_CONFIG_FILE)
      puts "Credentials.config file does not exist.  Please run 'rake create_credentials_config'"
      return
  end
  
  puts "Running integration tests"
  runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => INTEGRATION_TESTS_DIR, :results => TEST_REPORTS_DIR
  runner.executeTests ['com.mosso.cloudfiles.integration.tests']  
end

################
#  CREATING ZIP FILES
################

desc "Creates the downloadable zip files"
task :create_zips => [:clear_dist, :create_binary_zip, :create_source_zip, :create_doc_zip, :create_master_zip]

desc "Clear built zips"
task :clear_dist do
  FileUtils.rm_rf DEPLOY_DIR if File.exists? DEPLOY_DIR
  Dir.mkdir DEPLOY_DIR
end

desc "Create a binary zip"
task :create_binary_zip do
  puts "Creating binary zip"
  Dir.mkdir DEPLOY_BIN_DIR unless File.exists? DEPLOY_BIN_DIR
  create_zip("#{DEPLOY_BIN_DIR}/#{ZIP_FILE_PREFIX}-bin-#{RELEASE_BUILD_NUMBER}.zip", CONFIG_DIR ,  /UnitTests|IntegrationTests/)
end

desc "Creates a source zip"
task :create_source_zip do
  puts "Creating source zip"
  Dir.mkdir DEPLOY_SRC_DIR unless File.exists? DEPLOY_SRC_DIR
  create_zip("#{DEPLOY_SRC_DIR}/#{ZIP_FILE_PREFIX}-src-#{RELEASE_BUILD_NUMBER}.zip", Dir.pwd, /.gitignore|.git|build|dist|results|_ReSharper|bin|obj|.user|.suo|.resharper|.cache|Credentials.config/)
end

desc "Builds the API documentation and puts it in 'output'"
task :build_docs do
  puts "Creating docs"
  docu("#{CONFIG_DIR}/com.mosso.cloudfiles.dll")
end

desc "Creates a zip of the API documentation"
task :create_doc_zip => :build_docs do
  puts "Creating docs zip"
  Dir.mkdir DEPLOY_DOCS_DIR unless File.exists? DEPLOY_DOCS_DIR
  create_zip("#{DEPLOY_DOCS_DIR}/#{ZIP_FILE_PREFIX}-doc-#{RELEASE_BUILD_NUMBER}.zip", 'output/')
  FileUtils.rm_r('output') if File.exists? 'output'
end

desc "Create zip of binary, source, and doc zip files"
task :create_master_zip do
  puts "Creating master zip"
  create_zip("#{DEPLOY_DIR}/#{ZIP_FILE_PREFIX}-#{RELEASE_BUILD_NUMBER}.zip", DEPLOY_BIN_DIR)
  create_zip("#{DEPLOY_DIR}/#{ZIP_FILE_PREFIX}-#{RELEASE_BUILD_NUMBER}.zip", DEPLOY_SRC_DIR)
  create_zip("#{DEPLOY_DIR}/#{ZIP_FILE_PREFIX}-#{RELEASE_BUILD_NUMBER}.zip", DEPLOY_DOCS_DIR)
end