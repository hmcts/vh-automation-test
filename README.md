# VH-Automation-Test

This repo intends to run E2E automated tests for Video Hearings.

## Setup

Checkout the [VH Setup Repo](https://github.com/hmcts/vh-setup) and follow the instructions in the ReadMe.

Run the `setup-secrets-vh-automation.sh` script found in the `secrets` folder

> Note: You must ensure you have access to the appropriate KeyVault and subscription

## Running the tests

### Pre-populating with known data

Run the `seed_persons.sql` script found in the `setup/data` folder against an environment's Bookings API database.

> Note: Without this, the tests will fail as they rely on known data
