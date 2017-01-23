#!/usr/bin/python3

import os
import glob
import sys

if len(sys.argv)>1:
	searchFor = sys.argv[1]
	for filename in glob.iglob('./Diagram.SRC/**/*.cs', recursive=True):
		#print(filename)
		found = False
		with open(filename) as f:
			i = 0
			for line in f:
				i = i + 1 
				if line.find(searchFor) != -1:					
					os.system("subl "+filename+":"+str(i))
					found = True
					break
		if found:
			break
