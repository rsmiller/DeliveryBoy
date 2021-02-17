import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable()
export class DataService {
  private _HttpClient: HttpClient;
  private _BaseUrl: string;
  public CurrentIPAddress: string;
  public Lat: number = 0;
  public Long: number = 0;
  public Moving: boolean = false;
  private _TimerStarted: boolean = false;

  constructor(http: HttpClient, @Inject('BASE_URL_API') baseUrl: string) {
    this._HttpClient = http;
    this._BaseUrl = baseUrl;

    this.getIPAddress();
  }

  ngOnInit() {
    
  }

  public postTipData(t: string) {
    // adding some privacy. Using third decimal place = 365 foot area
    let new_lat = parseFloat(this.Lat.toFixed(4));
    let new_long = parseFloat(this.Long.toFixed(4));

    let new_lat_decimals = new_lat.toString().split(".")[1].length; 
    let new_long_decimals = new_long.toString().split(".")[1].length; 

    if (new_lat_decimals >= 4) {
      new_lat = parseFloat(new_lat.toString().substring(0, new_lat.toString().indexOf('.') + 4));
    }

    if (new_long_decimals >= 4) {
      new_long = parseFloat(new_long.toString().substring(0, new_long.toString().indexOf('.') + 4));
    }

    //console.log(this.Lat);
    //console.log(new_lat);
    //console.log(this.Long);
    //console.log(new_long);

    let postData = new TipModel();
    postData.Ip = this.CurrentIPAddress;
    postData.GeoLat = new_lat;
    postData.GeoLong = new_long;

    if (t == 'no') {
      postData.NoTip = true;
    }

    if (t == 'low') {
      postData.LowTip = true;
    }

    if (t == 'good') {
      postData.GoodTip = true;
    }

    if (t == 'great') {
      postData.GreatTip = true;
    }

    this._HttpClient.post<TipModel>(this._BaseUrl + 'Tip', postData).subscribe(result => {

      console.trace(result);
    }, error => {
      console.error(error);
    });
  }

  public getIPAddress() {

    this._HttpClient.get("https://api.ipify.org/?format=json").subscribe((res: any) => {
      this.CurrentIPAddress = res.ip;
    });
  }

  public getUserLocation() {
    navigator.geolocation.getCurrentPosition((position: Position) => {
      this.Lat = position.coords.latitude;
      this.Long = position.coords.longitude;

      if (this._TimerStarted == false) {
        this._TimerStarted = true;
        this.getLocationLoop();
      }

    }, function () {
      alert('We need access to your geo location to use the app')
    }, { timeout: 10000 })
  }

  private getLocationLoop() {
    setInterval(function () {
      navigator.geolocation.getCurrentPosition((position: Position) => {
        if (this.Lat != position.coords.latitude && this.Long != position.coords.longitude) {
          this.Moving = true;
        }
        else {
          this.Moving = false;
        }

        this.Lat = position.coords.latitude;
        this.Long = position.coords.longitude;
      }, function () {
          // Do nothing
          
      }, { timeout: 10000 })
    }, 20000);
  }
}

export class TipModel {
  Ip: string;
  GeoLat: number;
  GeoLong: number;

  Good: boolean = false;
  Bad: boolean = false;
  Nuetral: boolean = false;

  NoTip: boolean = false;
  LowTip: boolean = false;
  GoodTip: boolean = false;
  GreatTip: boolean = false;
}
