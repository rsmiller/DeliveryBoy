import { Component } from '@angular/core';
import { DataService } from '../DataService';

@Component({
  selector: 'add-component',
  templateUrl: './add-component.html',
  styleUrls: ['./add-component.css']
})
export class AddComponent {
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

  public IsMobile() {
    const toMatch = [
      /Android/i,
      /webOS/i,
      /iPhone/i,
      /iPad/i,
      /iPod/i,
      /BlackBerry/i,
      /Windows Phone/i
    ];

    return toMatch.some((toMatchItem) => {
      return navigator.userAgent.match(toMatchItem);
    });
  }
}
