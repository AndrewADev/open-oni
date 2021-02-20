#!/bin/bash

# NOTE: This is using a heuristic (existence of the directory)
# as a shortcut for validating the presence of the virtual 
# environment. Certain scenarios (failures during initial setup)
# can lead to incomplete/invalid virtual envs, where the folders 
# present, but without useful binaries & packages
# TODO: Consider more robust validation of virtual env here
# Create Python virtual environment if not present
if [ ! -d /opt/openoni/ENV/lib ]; then
  python3 -m venv ENV
fi

# Activate the Python virtual environment
source ENV/bin/activate || (echo "Failed to activate virtual environment"; exit -1;)

# Install Open ONI dependencies
# --no-cache-dir disables building local wheels as packages are installed
# Building wheels provides no benefit when isolated within virtual environment
pip install --no-cache-dir -r requirements.lock
