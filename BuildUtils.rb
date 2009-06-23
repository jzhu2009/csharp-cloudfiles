require 'rubygems'

require 'erb'
require 'activesupport'
require 'find'
require 'zip/zip'
require 'fileutils'

class NUnitRunner
	include FileTest

	def initialize(paths)
		@sourceDir = paths.fetch(:source, 'source')
		@resultsDir = paths.fetch(:results, 'results')
		@compilePlatform = paths.fetch(:platform, 'x86')
		@compileTarget = paths.fetch(:compilemode, 'debug')
		@nunitExe = File.join('lib', 'nunit', "nunit-console.exe").gsub('/','\\') + ' /nothread'
	end
	
	def executeTests(assemblies)		
		assemblies.each do |assem|
			file = File.expand_path("#{@sourceDir}/#{assem}.dll")
			sh "#{@nunitExe} \"#{file}\" /xml:#{@resultsDir}/#{assem}.xml"
		end
	end
end

class MSBuildRunner
	def self.compile(attributes)
		version = attributes.fetch(:clrversion, 'v3.5')
		compileTarget = attributes.fetch(:compilemode, 'debug')
	    solutionFile = attributes[:solutionfile]
		
		frameworkDir = File.join(ENV['windir'].dup, 'Microsoft.NET', 'Framework', version)
		msbuildFile = File.join(frameworkDir, 'msbuild.exe')
		
		sh "#{msbuildFile} #{solutionFile} /maxcpucount /verbosity:minimal /property:BuildInParallel=false /property:Configuration=#{compileTarget} /property:WarningLevel=0 /t:Rebuild"
	end
end

class AsmInfoBuilder
	attr_reader :buildnumber, :parameterless_attributes

	def initialize(version, properties)
		@properties = properties
		@buildnumber = version
		@properties['Version'] = @properties['InformationalVersion'] = buildnumber;
		@parameterless_attributes = [:allow_partially_trusted_callers]
	end
	
	def write(file)
		template = %q{using System.Reflection;
using System.Security;

<% @properties.each do |k, v| %>
<% if @parameterless_attributes.include? k %>
[assembly: <%= k.to_s.camelize %>]
<% else %>
[assembly: Assembly<%= k.to_s.camelize %>("<%= v %>")]
<% end %>
<% end %>
		}.gsub(/^    /, '')
		  
	  erb = ERB.new(template, 0, "%<>")
	  
	  File.open(file, 'w') do |file|
		  file.puts erb.result(binding) 
	  end
	end
end

class IntegrationTestsCredentialsFilesBuilder
	def write
		template = %q{<?xml version="1.0" encoding="utf-8"?>
    <credentials>
      <username>PUT USERNAME HERE</username>
      <api_key>PUT API KEY HERE</api_key>
    </credentials>
		}.gsub(/^    /, '')
		  
	  erb = ERB.new(template, 0, "%<>")
	  
	  File.open("com.mosso.cloudfiles.integration.tests/Credentials.config", 'w') do |file|
		  file.puts erb.result(binding) 
	  end
	end
end

def create_zip(filename, root, excludes=/^$/)
  root = root + "/" if root[root.length - 1].chr != "/"
  Zip::ZipFile.open(filename, Zip::ZipFile::CREATE) do |zip|
    Find.find(root) do |path|
      next if path =~ excludes
	  
      zip_path = path.gsub(root, '')
      zip.add(zip_path, path)
    end
  end
end

def docu(dll_name)
	FileUtils.rm_r('output') if File.exists? 'output'
	
	docu_exe = "lib/docu/docu.exe"
	
	`#{docu_exe} #{dll_name}`
end