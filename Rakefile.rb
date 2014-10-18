#!/usr/bin/env ruby

require 'albacore'
require 'fileutils'

CONFIG = 'Debug'
RAKE_DIR = File.expand_path(File.dirname(__FILE__))
SOLUTION_DIR = RAKE_DIR
SOLUTION_FILE = 'James.Testing.sln'
NUGET = SOLUTION_DIR + "/.nuget/nuget.exe"

task :default => ['build:msbuild']
# task :test => ['build:mstest' ]
task :package => ['package:packall']
task :push => ['package:pushall']

namespace :build do

  msbuild :msbuild, [:targets] do |msb, args|
    args.with_defaults(:targets => :Build)
    msb.properties :configuration => CONFIG
    msb.targets args[:targets]
    msb.solution = "#{SOLUTION_DIR}/#{SOLUTION_FILE}"
  end
  
end

namespace :package do

	def create_packs()
		create_pack 'James.Testing'
		create_pack 'James.Testing.Rest'
		create_pack 'James.Testing.Wcf'
		create_pack 'James.Testing.Pdf'
		create_pack 'James.Abstractions.System'
	end

	def create_pack(name)
		puts
		puts "pack '" + name + "'? (Y/N)"
		answer = STDIN.gets.strip
		if (answer == "Y" or answer == "y")
			sh NUGET + ' pack ' + name + '/' + name + '.csproj -o pack'
		end
	end

	task :packall => [ :clean ] do
		Dir.mkdir('pack')
		create_packs	
		Dir.glob('pack/*') { |file| FileUtils.move(file,'nuget/') }
		Dir.rmdir('pack')
	end

	task :pushall => [ :clean ] do

		puts "Please enter the project's NuGet API Key:"
		key = STDIN.gets.strip
		sh NUGET + ' setApiKey ' + key

		Dir.mkdir('pack')
		create_packs	
		Dir.chdir('pack')
		Dir.glob('*').each do |file|
			sh NUGET + ' push ' + file
			FileUtils.move(file,'nuget/')
		end
		Dir.chdir('..')
		Dir.rmdir('pack')
	end

	task :clean do
		if Dir.exists? 'pack'
			FileUtils.remove_dir 'pack', force = true
		end
	end
end