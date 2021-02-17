import { Component } from '@angular/core';
import { DataService } from '../DataService';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public _DataService: DataService;
  public Saved: boolean = false;

  constructor(dataService: DataService) {

    this._DataService = dataService;
  }

  ngOnInit() {
    this._DataService.getUserLocation();
  }

  public postTipData(t: string) {
    this._DataService.postTipData(t);
    this.Saved = true;

    setTimeout(function () {
      this.Saved = false;
    }.bind(this), 5000);
  }
}
