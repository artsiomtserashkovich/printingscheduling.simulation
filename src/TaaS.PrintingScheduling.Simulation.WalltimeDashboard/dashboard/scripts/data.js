let data = {
	"name": "genome-dax-0",
	"workflow": {
		"jobs": [
			{
				"avgCPU": 4.4805,
				"name": "create_dir_genome-dax_0_execution",
				"type": "auxiliary",
				"bytesWritten": 307833,
				"machine": "runs",
				"parents": [],
				"memory": 35904.0,
				"runtime": 2.098,
				"bytesRead": 1606295
			}
		],
		"machines": [
			{
				"nodeName": "runs",
				"system": "linux",
				"architecture": "x86_64",
				"memory": 131793812,
				"release": "3.10.0-229.4.2.el7.x86_64",
				"cpu": {
					"count": 48,
					"vendor": "GenuineIntel",
					"speed": 2265
				}
			}
		]
	},
	"wms": {
		"url": "http://pegasus.isi.edu",
		"version": "4.6.2",
		"name": "Pegasus"
	},
	"author": {
		"name": "Rafael Ferreira da Silva",
		"email": "rafsilva@isi.edu"
	},
	"schemaVersion": "0.2",
	"createdAt": "2016-11-29T20:00:00.224164Z",
	"description": "$3"
}
let energyData = []
let currGraphState = "taskView";
let currZoomState = {};
let hostColours = {}
let currentlySelectedHost = {hostName: "", id: ""}
let firstVisit
