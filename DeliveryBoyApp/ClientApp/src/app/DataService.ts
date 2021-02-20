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
  public MovingCountRequired: number = 2;
  public MovingCount: number = 0;

  private _TimerStarted: boolean = false;

  public TipDataReceived = new BehaviorSubject<TipDto>(undefined);

  constructor(http: HttpClient, @Inject('BASE_URL_API') baseUrl: string) {
    this._HttpClient = http;
    this._BaseUrl = baseUrl;

    this.getIPAddress();
  }

  ngOnInit() {
    
  }

  public postTipData(t: string) {
    // adding some privacy. Using third decimal place = 365 foot area
    let new_lat = parseFloat(this.Lat.toFixed(3));
    let new_long = parseFloat(this.Long.toFixed(3));

    let new_lat_decimals = new_lat.toString().split(".")[1].length; 
    let new_long_decimals = new_long.toString().split(".")[1].length; 

    if (new_lat_decimals >= 4) {
      new_lat = parseFloat(new_lat.toString().substring(0, new_lat.toString().indexOf('.') + 3));
    }

    if (new_long_decimals >= 4) {
      new_long = parseFloat(new_long.toString().substring(0, new_long.toString().indexOf('.') + 3));
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
      //console.trace(result);
      location.reload();

    }, error => {
      console.error(error);
    });
  }

  public getTipData(streetNumber: number, streetName: string, zipcode: number) {

    this._HttpClient.get<TipDto>(this._BaseUrl + 'Tip?streetName=' + streetName + '&streetNumber=' + streetNumber + '&zipcode=' + zipcode).subscribe(result => {
      console.trace(result);

      this.TipDataReceived.next(result);

    }, error => {
      console.error(error);
    });
  }

  public getIPAddress() {

    this._HttpClient.get("https://api.ipify.org/?format=json").subscribe((res: any) => {
      this.CurrentIPAddress = res.ip;
      if (this.CurrentIPAddress == null) {
        this.getUnsecuredIPAddress();
      }

    }, error => {
        this.getUnsecuredIPAddress();
    });
  }

  public getUnsecuredIPAddress() {

    this._HttpClient.get("http://api.ipify.org/?format=json").subscribe((res: any) => {
      this.CurrentIPAddress = res.ip;
    }, error => {
      console.error(error);
    });
  }

  public getUserLocation() {

    if (navigator.geolocation) {

      navigator.geolocation.watchPosition((position: Position) => {
        if (this.Lat != parseFloat(position.coords.latitude.toFixed(3)) || this.Long != parseFloat(position.coords.longitude.toFixed(3))) {
          this.Moving = true;
          this.MovingCount = 1;
        }
        else {
          // Check if still moving by recording counts
          if (this.MovingCount >= this.MovingCountRequired) {
            this.Moving = false;
          }
          else {
            this.MovingCount++;
          }
        }

        //this.Lat = position.coords.latitude;
        //this.Long = position.coords.longitude;
        this.Lat = parseFloat(position.coords.latitude.toFixed(3));
        this.Long = parseFloat(position.coords.longitude.toFixed(3));
      }, function () {
        // Do nothing

      }, { timeout: 10000 });

      //navigator.geolocation.getCurrentPosition((position: Position) => {
      //  //this.Lat = position.coords.latitude;
      //  //this.Long = position.coords.longitude;

      //  this.Lat = parseFloat(position.coords.latitude.toFixed(3));
      //  this.Long = parseFloat(position.coords.longitude.toFixed(3));

      //  if (this._TimerStarted == false) {
      //    this._TimerStarted = true;
      //    this.getLocationLoop();
      //  }

      //}, function () {
      //    alert("Geo location must me turned on to use this site");
      //}, { timeout: 10000 })
    }
    else {

    }
  }

  private getLocationLoop() {
    navigator.geolocation.watchPosition((position: Position) => {
      if (this.Lat != parseFloat(position.coords.latitude.toFixed(3)) || this.Long != parseFloat(position.coords.longitude.toFixed(3))) {
        this.Moving = true;
      }
      else {
        this.Moving = false;
      }

      //this.Lat = position.coords.latitude;
      //this.Long = position.coords.longitude;
      this.Lat = parseFloat(position.coords.latitude.toFixed(3));
      this.Long = parseFloat(position.coords.longitude.toFixed(3));
    }, function () {
      // Do nothing

    }, { timeout: 10000 });
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

export class GetTipModel {
  streetNumber: number;
  streetName: string;
  zipcode: number;
}

export class TipDto {
  noTipPercentage: number = 0;
  lowTipPercentage: number = 0;
  goodTipPercentage: number = 0;
  greatTipPercentage: number = 0;
}
