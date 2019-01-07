import {Directive, ElementRef, Input} from '@angular/core';
import {Network} from 'vis'

@Directive({
  selector: '[appGraphVis]'
})
export class GraphVisDirective {

  network;

  constructor(private el: ElementRef) {
  }

  @Input() set appGraphVis(graphData) {
    console.log('graph data ', graphData);
    //const options = {};
    const options = {
      edges: {
        smooth: {
          forceDirection: "none"
        },
        //font: {align: 'top'}
        font: {
          color: '#eeeeee',
          strokeWidth: 0
        }
      },
      physics: {
        barnesHut: {
          gravitationalConstant: -10200,
          springLength: 85
        },
        minVelocity: 0.75
      },
      nodes: {
        shape: 'dot',
        font: {
          color: '#eeeeee'
        }
        /*scaling: {
          label: {
            min: 8,
            max: 20
          }
        }*/
      }
    };

    if (!this.network) {
      this.network = new Network(this.el.nativeElement, graphData, options);

      this.network.on("click", function (params) {
        params.event = "[original event]";
        document.getElementById('eventSpan').innerHTML = '<h2>Click event:</h2>' + JSON.stringify(params, null, 4);
        const selectedId = this.getNodeAt(params.pointer.DOM);
        console.log('click event, getNodeAt returns: ' + this.getNodeAt(params.pointer.DOM));
        console.log(graphData["nodes"]._data[selectedId]);
      });
    }

  }

}
