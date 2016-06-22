class ExStrataModel {
    constructor(params = {}) {

        //set up this.scene
        this.scene = new THREE.Scene();
        this.camera = new THREE.PerspectiveCamera(75, $('#renderView').width() / $('#renderView').height(), 0.1, 10000);
        this.fullScreen = false;
        this.mobile = this.mobileAndTabletcheck();

        this.camera.position.x = params.cameraX || -125;
        this.camera.position.y = params.cameraY || -3;
        this.camera.position.z = params.cameraZ || 45;
        this.camera.lookAt(new THREE.Vector3(0, 0, 0));

        this.renderer = new THREE.WebGLRenderer({
            alpha: true,
            antialias: params.antialias || true
        });

        let that = this;

        this.controls = new THREE.OrbitControls(this.camera, $('#renderView')[0]);
        this.controls.addEventListener('change', function() {
            that.render(that.renderer);
        });

        this.renderer.shadowMap.enabled = true;
        this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;

        this.renderer.gammaInput = true;
        this.renderer.gammaOutput = true;

        this.renderer.setSize($('#renderView').width(), $('#renderView').height());
        $('#renderView').append(this.renderer.domElement);

        this.scene.fog = new THREE.FogExp2(0x050423, 0.002);


        this.poleGeometry = new THREE.CylinderGeometry(2, 2, 160, 10);
        this.poleMaterial = new THREE.MeshStandardMaterial({
            color: 0xeeeeee,
            metalness: .8
        });
        this.pole = new THREE.Mesh(this.poleGeometry, this.poleMaterial);
        this.scene.add(this.pole);

        this.pole.castShadow = true;


        let floorTextureLoader = new THREE.TextureLoader();
        floorTextureLoader.load(
            params.floorPath || 'img/tiles2.jpg',
            function(texture) {
                let floorGeometry = new THREE.PlaneGeometry(5000, 5000);

                texture.wrapS = texture.wrapT = THREE.RepeatWrapping;
                texture.repeat.set(90, 90);

                let floorMaterial = new THREE.MeshStandardMaterial({
                    map: texture
                });
                that.floor = new THREE.Mesh(floorGeometry, floorMaterial);
                that.floor.rotateX(-Math.PI / 2);
                that.floor.position.y = -95;
                that.floor.receiveShadow = true;
            }
        );


        let baseTextureLoader = new THREE.TextureLoader();
        baseTextureLoader.load(
            'img/concrete.jpg',
            function(texture) {
                let baseGeometry = new THREE.BoxGeometry(90, 15, 50);
                texture.wrapS = texture.wrapT = THREE.RepeatWrapping;
                texture.repeat.set(2, 2, 0.1);

                that.baseMaterial = new THREE.MeshStandardMaterial({
                    map: texture
                });

                that.base = new THREE.Mesh(baseGeometry, that.baseMaterial);
                that.base.position.y = -87;

                that.base.receiveShadow = true;
                that.base.castShadow = true;
            }
        );

        //datGUI  = new dat.GUI()


        this.rings = [];
        this.glows = [];
        this.buildScene({
            glow: !this.mobile,
            base: false,
            floor: false
        });

        //this.setTime('night');
        //this.fromFile('gradients');

        // light = new THREE.PointLight( 0xffffff, 1, 0 );
        // light.position.set( 0,0,5 );
        // light.castShadow=true;
        // this.scene.add( light );

        this.dirLight = new THREE.DirectionalLight(0xffffff);
        this.dirLight.position.set(1, 1, 1);
        this.dirLight.castShadow = true;

        this.dirLight.shadow.camera.left = -100;
        this.dirLight.shadow.camera.right = 100;
        this.dirLight.shadow.camera.top = 100;
        this.dirLight.shadow.camera.bottom = -100;
        this.dirLight.intensity = .3

        this.scene.add(this.dirLight);

        this.blueDirLight = new THREE.DirectionalLight(0x002288);
        this.blueDirLight.position.set(-1, -1, -1);
        this.scene.add(this.blueDirLight);

        this.ambLight = new THREE.AmbientLight(0x8e7001);
        this.ambLight.intensity = .2;
        this.scene.add(this.ambLight);


        //this.render
        this.performance = 0; //lower is better
        this.render(this.renderer);

        this.hdriSphere = {};
        this.patternPlay = -1;

        $(window).resize(function() {
            that.onWindowResize(that);
        });
    }


    buildScene(settings = {}) {

        if (settings.base || false) {
            this.scene.add(this.base);
        }
        if (settings.floor || false) {
            this.scene.add(this.floor);
        }

        //EX STRATA:
        let shape = [54.5894, 88.5438, 109.782, 124.964, 136.132, 144.222, 149.733, 152.944, 151, 148.917, 146.834, 144.752, 142.669, 140.586, 138.503, 136.421, 134.338, 132.255, 130.172, 128.09, 126.007, 123.924, 121.841, 119.759, 117.676, 115.593, 113.51, 111.428, 109.345, 107.262, 105.179, 103.097, 101.014, 98.931, 96.8483, 94.7655, 92.6828, 90.6, 88.5172, 86.4345, 84.3517, 82.269, 80.1862, 78.1034, 76.0207, 73.9379, 71.8552, 69.7724, 67.6897, 65.6069, 63.5241, 61.4414, 59.3586, 57.2759, 55.1931, 53.1103, 51.0276, 48.9448, 46.8621, 44.7793, 42.6966, 40.6138, 38.531, 36.4483, 34.3655, 32.2828, 45.607, 79.3221, 99.2774, 113.013, 122.638, 129.074, 132.786, 134, 132.786, 129.074, 122.638, 113.013, 99.2774, 79.3221];

        let outerRings = [];
        for (let i = shape.length - 1; i >= 0; i--) {


            let geometry = new THREE.TorusGeometry(shape[i] / 9, .2, 6, 50);
            outerRings.push(new THREE.Mesh(geometry, new THREE.MeshStandardMaterial({
                color: 0xeeeeee,
                emissive: 0x000000,
                metalness: 0.7
            })));

            let innerGeometry = new THREE.TorusGeometry(3, .5, 6, 30);
            this.rings.push(new THREE.Mesh(innerGeometry, new THREE.MeshStandardMaterial({
                color: 0xeeeeee,
                emissive: 0x000000,
                metalness: 1
            })));

            if (settings.glow || false) {
                let glowMesh = new THREEx.GeometricGlowMesh(this.rings[this.rings.length - 1]);

                glowMesh.insideMesh.material.uniforms.glowColor.value = new THREE.Color(0x000000);
                glowMesh.insideMesh.material.uniforms.coeficient.value = 1;
                glowMesh.insideMesh.material.uniforms.power.value = 2;

                glowMesh.outsideMesh.material.uniforms.glowColor.value = new THREE.Color(0x000000);
                glowMesh.outsideMesh.material.uniforms.coeficient.value = 0.5;
                glowMesh.outsideMesh.material.uniforms.power.value = 4;

                this.rings[this.rings.length - 1].add(glowMesh.object3d);

                this.glows.push(glowMesh);
            }

            // example of customization of the default glowMesh

            //outerRings[outerRings.length-1].receiveShadow=true;
            outerRings[outerRings.length - 1].castShadow = true;

            outerRings[outerRings.length - 1].rotateX(Math.PI / 2);
            outerRings[outerRings.length - 1].position.y = shape.length + i * -2;
            this.rings[this.rings.length - 1].rotateX(Math.PI / 2);
            this.rings[this.rings.length - 1].position.y = shape.length + i * -2;
            this.scene.add(outerRings[outerRings.length - 1]);
            this.scene.add(this.rings[this.rings.length - 1]);
        }
    }

    toggleFullScreen() {
        var resizeI = 0;
        let that = this;
        if (this.fullScreen) {

            this.scene.remove(this.floor);
            this.scene.remove(this.base);
            this.scene.remove(this.hdriSphere);

            setTimeout(function() {
                $('#renderView').css('width', '300px');
                $('#controlPanel').css('width', 'calc(100% - 450px)');
                $('.APICard, .newCard').css('width', 'calc(50% - 30px)');
                var resizing = self.setInterval(function() {
                    if (resizeI > 200 / 33) {
                        clearInterval(resizing);
                    }
                    that.onWindowResize(that);
                }, 33);
            }, 150);
        } else {


            this.scene.add(this.floor);
            this.scene.add(this.base);
            this.setTime('day');

            setTimeout(function() {
                $('#renderView').css('width', '100%');
                $('#controlPanel').css('width', '500px');
                $('.APICard, .newCard').css('width', 'calc(100% - 30px)');
                var resizing = self.setInterval(function() {
                    if (resizeI > 200 / 33) {
                        clearInterval(resizing);
                    }
                    that.onWindowResize(that);
                }, 33);
            }, 150);
        }
        this.fullScreen = !this.fullScreen;
    }

    setTime(time) {
        if (this.hdriSphere.material === undefined)
            this.scene.remove(this.hdriSphere);
        if (time == 'night') {
            this.setHDRI("img/hdri8.jpg", 1000);

            this.scene.fog.density = 0.00025;
            this.scene.fog.color = new THREE.Color(0x050423);
            this.ambLight.intensity = .2;
            this.ambLight.color = new THREE.Color(0x8e7001);
            this.dirLight.intensity = .3;
        } else {
            let geometry = new THREE.SphereGeometry(3000, 32, 32);
            // do something with the texture
            let material = new THREE.MeshBasicMaterial();
            this.hdriSphere = new THREE.Mesh(geometry, material);
            this.hdriSphere.material.side = THREE.BackSide;
            this.scene.add(this.hdriSphere);

            this.scene.fog.density = 0.002;
            this.scene.fog.color = new THREE.Color(0x7ba3f1);
            this.ambLight.intensity = 1;
            this.ambLight.color = new THREE.Color(0xffffff);
            this.dirLight.intensity = 1;
        }
        this.render(this.renderer);
    }


    increasePerformance() {
        this.performance++;
        switch (this.performance) {
            case 1:
                this.dirLight.castShadow = false;
                break;
            case 2:
                this.scene.remove(this.blueDirLight);
                break;
            case 3:
                this.scene.remove(this.ambLight);
                break;
            case 4:
                this.scene.fog.density = 0;
                break;
            case 5:
                this.scene.remove(this.base);
                this.scene.remove(this.floor);
                break;
            case 6:
                this.scene.remove(this.dirLight);
                break;
            case 7:
                this.renderer.setPixelRatio(.5);
                this.renderer.setSize($('#renderView').width(), $('#renderView').height());
                break;
            default:
                this.performance = 7;
                alert('Graphics settings cannot be turned down more');
                break;
        }
        this.render(this.renderer);
    }

    decreasePerformance() {
        if (this.performance > 0) {
            this.performance--;
            switch (this.performance) {
                case 0:
                    this.dirLight.castShadow = true;
                    break;
                case 1:
                    this.scene.add(this.blueDirLight);
                    break;
                case 2:
                    this.scene.add(this.ambLight);
                    break;
                case 3:
                    this.scene.fog.density = 0.002;
                    break;
                case 4:
                    this.scene.add(this.base);
                    this.scene.add(this.floor);
                    break;
                case 5:
                    this.scene.add(this.dirLight);
                    break;
                case 6:
                    this.renderer.setPixelRatio(1);
                    this.renderer.setSize($('#renderView').width(), $('#renderView').height());
                    break;
                default:
                    console.log('error');
                    break;
            }
            this.render(this.renderer);
        }
    }

    onWindowResize(scope) {
        scope.camera.aspect = $('#renderView').width() / $('#renderView').height();
        scope.camera.updateProjectionMatrix();

        scope.renderer.setSize($('#renderView').width(), $('#renderView').height());
        scope.render(scope.renderer);
    }

    render(renderer) {
        renderer.render(this.scene, this.camera);
    }

    fromFile(name) {
        let that = this;
        $.get('patterns/' + name + '.pattern').done(function(response) {
            that.fromPattern(response);
        });
    }

    fromPattern(pattern) {
        pattern = pattern.split('&');

        let delays = pattern.filter(x => x.indexOf('ms') != -1);
        delays = delays.map(x => Number(x.split('ms]=')[1]));

        let frames = [];
        for (let i = 0; i < delays.length; i++) {
            frames.push(pattern.filter(x => x.indexOf("[frames][" + i + ']') != -1 && x.indexOf('ms') == -1));
            frames[i] = frames[i].map(x => x.split(']=')[1].replace(/\n/, '').split(','));
        }
        frames.pop(); //zwarte laatste frame eraf
        delays.pop();

        console.debug('delays: ',delays);

        if (this.patternPlay != -1)
            clearTimeout(this.patternPlay);
        this.showPattern(frames, delays, 0);
    }

    showPattern(frames, delays, index) {
        console.log("Showing frame "+(index+1)+" out of "+delays.length);

        let colours = frames[index];
        for (let i = 0; i < colours.length; i++) {
            let colour = new THREE.Color(colours[i][0] / 255, colours[i][1] / 255, colours[i][2] / 255);
            this.rings[i].material.emissive = colour;
            if (this.glows.length > i) {
                this.glows[i].outsideMesh.material.uniforms.glowColor.value = colour;
                this.glows[i].insideMesh.material.uniforms.glowColor.value = colour;
            }
        }
        this.render(this.renderer);

        if (index < delays.length - 1) {
            let that = this;
            this.patternPlay = self.setTimeout(function() {
                that.showPattern(frames, delays, ++index);
            }, delays[index] - (index > 0 ? delays[index - 1] : 0));
        }
    }


    setHDRI(path, size) {
        this.renderer
            // instantiate a loader
        let loader = new THREE.TextureLoader();

        // load a resource
        loader.load(
            // resource URL
            path,
            // Function when resource is loaded
            function(texture) {
                let geometry = new THREE.SphereGeometry(size, 32, 32);
                // do something with the texture
                let material = new THREE.MeshBasicMaterial({
                    map: texture
                });
                this.hdriSphere = new THREE.Mesh(geometry, material);
                this.hdriSphere.material.side = THREE.BackSide;
                this.scene.add(this.hdriSphere);
                this.render();
            },
            // Function called when download progresses
            function(xhr) {
                console.log((xhr.loaded / xhr.total * 100) + '% loaded');
            },
            // Function called when download errors
            function(xhr) {
                console.log('An error occured while loading the hdri image');
            }
        );
    }

    mobileAndTabletcheck() {
        var check = false;
        (function(a) {
            if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true
        })(navigator.userAgent || navigator.vendor || window.opera);
        return check;
    }
}
