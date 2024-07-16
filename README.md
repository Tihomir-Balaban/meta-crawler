# Meta Crawler

## Description
A command-line application to crawl and parse data from https://www.telered.com.ar/buscador-grilla.

## Commands
- `list` - Returns a list of channels.
- `channel {channel_number}` - Returns TV listings for the specified channel.
- `json {channel_number}` - Saves TV listings to a JSON file.

## Usage
```sh
Meta.Crawler.exe list
Meta.Crawler.exe channel 8
Meta.Crawler.exe json 7
