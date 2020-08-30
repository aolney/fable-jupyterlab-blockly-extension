module Logging

open Fable.Core
open Fable.Core.JsInterop
open Thoth.Json 
open Thoth.Fetch

//Fable 2 transition 
let inline toJson x = Encode.Auto.toString(4, x)
let inline ofJson<'T> json = Decode.Auto.unsafeFromString<'T>(json)

type BlocklyLogEntry082720 =
    {
        schema: string
        name: string
        id : string //Event doc suggests this can be list at times, but foreign interface doesn't reflect that: https://developers.google.com/blockly/guides/configure/web/events
    } with
    static member Create name id = { schema = "ble082720"; name = name; id=id }

type JupyterLogEntry082720 =
    {
        schema: string
        name: string
        payload : string option
    } with
    static member Create name payload = { schema = "jle082720"; name = name ; payload = payload}

type LogEntry = 
    {
        username: string
        json: string
    }

//No logging by default; only turn on if within olney.ai domain;
let mutable shouldLog = false
let LogServerEndpoint = "https://logging.olney.ai"
/// Log to server. Basically this is Express wrapping a database, but repo is not public as of 8/25/20
let LogToServer( logObject: obj ) = 
    if shouldLog then
        promise {
            let username = Browser.Dom.window.location.href
            do! Fetch.post( LogServerEndpoint + "/datawhys/log" , { username=username; json=toJson(logObject) } ) //caseStrategy = SnakeCase
        } |> ignore

/// Call this when attaching extension
let CheckShouldLog() =
    if Browser.Dom.window.location.href.Contains("olney.ai") then
        shouldLog <- true