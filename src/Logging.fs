module Logging

open Fable.Core
open Fable.Core.JsInterop
open Thoth.Json 
open Thoth.Fetch

//Fable 2 transition 
let inline toJson x = Encode.Auto.toString(4, x)
let inline ofJson<'T> json = Decode.Auto.unsafeFromString<'T>(json)

//Circular dependencies break Thoth autoencoding, however, it seems this only happens with SVG data we don't want
// [<Import("stringify", from="flatted")>]
// let flattedStringify(x: obj): string = jsNative


type BlocklyLogEntry082720 =
    {
        schema: string
        name: string
        object : obj //for now, logging everything: https://developers.google.com/blockly/guides/configure/web/events
    } with
    static member Create name object = { schema = "ble082720"; name = name; object=object }

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

/// Where we are sending the data
let mutable logUrl : string option = None
 
let mutable idOption : string option = None

//Filter out problematic JSON, e.g. circular references, on a case-by-base basis (vs npm flatted)
let filterJson ( o : obj ) =
    //on Blockly UI drag events, remove circular refs and data we don't want anyways
    if not <| (o?object=null)  && not <| (o?object?element=null) && string(o?object?element).StartsWith("drag") then
        o?object?newValue <- null
        o?object?oldValue <- null
    toJson( o )

/// Log to server if we've been given a logUrl. Basically this is Express wrapping a database, but that repo is not public as of 12/29/20
let LogToServer( logObject: obj ) = 
    match logUrl with
    | Some(url) ->
        promise {
            let username = 
                match idOption with
                | Some(id) -> id
                //In a JupyterHub situation, we can use the username embedded in the url
                | None -> Browser.Dom.window.location.href
            do! Fetch.post( url, { username=username; json=filterJson(logObject) } ) //caseStrategy = SnakeCase
            // do! Fetch.post( url, { username=username; json=flattedStringify(logObject) } ) //caseStrategy = SnakeCase
            // do! Fetch.post( url, { username=username; json=toJson(logObject) } )
        } |> ignore
    | None -> ()