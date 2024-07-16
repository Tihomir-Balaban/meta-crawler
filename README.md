# Meta Crawler

## Description
A command-line application to crawl and parse data from https://www.telered.com.ar/buscador-grilla.

## Commands
- `list` - Returns a list of channels.
- `channel {channel_number}` - Returns TV listings for the specified channel.
- `json {channel_number}` - Saves TV listings to a JSON file.

## Usage
```sh
dotnet run list
dotnet run channel
dotnet run json
